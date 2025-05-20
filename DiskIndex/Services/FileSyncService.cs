using System.Text.Json;
using DiskIndex.Models;
using Microsoft.EntityFrameworkCore;

namespace DiskIndex.Services
{
    public class FileSyncService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Dictionary<string, string> _categoryPaths;       

        public FileSyncService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        private string GetCategoryFromPath(string filePath, Dictionary<string, string> categoryPaths)
        {
            foreach (var kvp in categoryPaths)
            {
                if (filePath.StartsWith(kvp.Value, StringComparison.OrdinalIgnoreCase))
                {
                    return kvp.Key;
                }
            }
            return "Unknown";
        }

        // This is the main system to keep the database in sync with the disk. Requires a string with the root folder path.
        // IMPORTANT: The folder path can be changed by editing the rootFolderPath.json file located in the Configs folder.

        // It has a set of steps that makes sure the resources in the DB are updated if a file is added, removed or modified.
        public async Task RefreshDatabaseAsync(string rootFolder)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FileDbContext>();

            var json = File.ReadAllText("Configs\\categories.json");
            _categoryPaths = JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                             ?? new Dictionary<string, string>();

            // 1. Loads ALL the files from the database
            var dbFiles = await context.Files.ToListAsync();
            var dbFilesDict = dbFiles.ToDictionary(f => f.Path, f => f);

            // 2. Finds the files on disk (disk root location is passed as a string called "rootFolder")
            try
            {
                if (string.IsNullOrEmpty(rootFolder) || !Directory.Exists(rootFolder))
                {
                    throw new DirectoryNotFoundException("The specified root folder does not exist or the JSON value is not correct. Check the Configs\\rootFolderPath.json");
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            var diskFiles = Directory.GetFiles(rootFolder, "*", SearchOption.AllDirectories);
            var pathsOnDisk = new HashSet<string>(diskFiles, StringComparer.OrdinalIgnoreCase);

            // 3. Adds or updates files in the database
            foreach (var filePath in diskFiles)
            {
                var info = new FileInfo(filePath);
                var category = GetCategoryFromPath(filePath, _categoryPaths);

                if (dbFilesDict.TryGetValue(filePath, out var existingRecord))
                {
                    if (existingRecord.LastModified != info.LastWriteTime || existingRecord.Size != info.Length)
                    {
                        existingRecord.LastModified = info.LastWriteTime;
                        existingRecord.Size = info.Length;
                        context.Files.Update(existingRecord);
                    }
                }
                else
                {
                    context.Files.Add(new FileRecord
                    {
                        Name = info.Name,
                        Category = category, // You can edit the categories as you wish with the JSON file located in "Configs\categories.json"   
                        Path = info.FullName,
                        Type = info.Extension,
                        Size = info.Length,
                        LastModified = info.LastWriteTime,
                        IsNSFW = "Unknown"
                    });
                }
            }

            // 4. Delete files from the database that are not on disk anymore
            foreach (var dbFile in dbFiles)
            {
                if (!pathsOnDisk.Contains(dbFile.Path))
                {
                    context.Files.Remove(dbFile);
                }
            }

            // 5. Save
            await context.SaveChangesAsync();
        }
    }
}


// FileService: Not in use anymore. Could be useful in the future if a manual scan feature is needed.
// Now the system works with a background service that scans the disk and updates the database. Check FileSyncService.cs for more information.


namespace DiskIndex.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class FileService
    {
        private readonly string rootDirectory;

        public FileService(string rootDirectory)
        {
            this.rootDirectory = rootDirectory;
        }

        public IEnumerable<FileItem> GetFiles()
        {
            // Checks if the root directory exists
            if (!Directory.Exists(rootDirectory))
                throw new DirectoryNotFoundException($"Directory not found: {rootDirectory}");

            // Gather files and folders in the root directory
            var files = Directory.GetFiles(rootDirectory);
            var directories = Directory.GetDirectories(rootDirectory);

            // File list
            var fileItems = files.Select(file => new FileItem
            {
                Name = Path.GetFileName(file),
                Path = file,
                Type = "File",
                Size = new FileInfo(file).Length,
                LastModified = File.GetLastWriteTime(file)
            }).ToList();

            // Adds directories to the file list
            fileItems.AddRange(directories.Select(directory => new FileItem
            {
                Name = Path.GetFileName(directory),
                Path = directory,
                Type = "Directory",
                Size = 0,
                LastModified = Directory.GetLastWriteTime(directory)
            }));

            return fileItems;
        }
    }

    public class FileItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
    }

}

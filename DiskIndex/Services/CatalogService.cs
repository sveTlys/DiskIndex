using DiskIndex.Models;
using Microsoft.EntityFrameworkCore;

namespace DiskIndex.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly FileDbContext _dbContext;
        public CatalogService(FileDbContext context)
        {
            _dbContext = context;
        }

        public FileRecord GetRecordById(int id)
        {
            return _dbContext.Files.FirstOrDefault(f => f.Id == id);
        }
        public List<FileRecord> GetRecords()
        {
            return _dbContext.Files.ToList();
        }

        public async Task<(IEnumerable<FileRecord> Records, int TotalCount)> GetPagedRecordsAsync(int skip, int take, string searchString, string? sortField, bool descending)
        {
            var query = _dbContext.Files.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(f =>
                    f.Name.Contains(searchString) ||
                    f.Type.Contains(searchString) ||
                    f.Path.Contains(searchString) ||
                    f.Category.Contains(searchString) ||
                    f.LastModified.ToString().Contains(searchString));
            }

            // Sorting
            query = (sortField, descending) switch
            {
                (nameof(FileRecord.Name), true) => query.OrderByDescending(f => f.Name),
                (nameof(FileRecord.Name), false) => query.OrderBy(f => f.Name),
                (nameof(FileRecord.Type), true) => query.OrderByDescending(f => f.Type),
                (nameof(FileRecord.Type), false) => query.OrderBy(f => f.Type),
                (nameof(FileRecord.Category), true) => query.OrderByDescending(f => f.Category),
                (nameof(FileRecord.Category), false) => query.OrderBy(f => f.Category),
                (nameof(FileRecord.Size), true) => query.OrderByDescending(f => f.Size),
                (nameof(FileRecord.Size), false) => query.OrderBy(f => f.Size),
                (nameof(FileRecord.LastModified), true) => query.OrderByDescending(f => f.LastModified),
                (nameof(FileRecord.LastModified), false) => query.OrderBy(f => f.LastModified),
                _ => query.OrderBy(f => f.Name) // fallback
            };

            int total = await query.CountAsync();
            var records = await query.Skip(skip).Take(take).ToListAsync();

            return (records, total);
        }

        public Task<int> CountAllFilesAsync()
        {
            return _dbContext.Files.CountAsync();
        }
    }
}

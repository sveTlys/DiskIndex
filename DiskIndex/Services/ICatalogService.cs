using DiskIndex.Models;

namespace DiskIndex.Services
{
    public interface ICatalogService
    {
        List<FileRecord> GetRecords(); // Returns all records in the database

        // Takes the record sets and returns a paginated list of records
        Task<(IEnumerable<FileRecord> Records, int TotalCount)> GetPagedRecordsAsync(int skip, int take, string searchString, string? sortField, bool descending);

        Task<int> CountAllFilesAsync();

    }
}

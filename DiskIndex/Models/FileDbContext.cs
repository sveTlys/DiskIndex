using DiskIndex.Models;
using Microsoft.EntityFrameworkCore;

public class FileDbContext : DbContext
{
    public DbSet<FileRecord> Files { get; set; }

    public FileDbContext(DbContextOptions<FileDbContext> options) : base(options) { }
}


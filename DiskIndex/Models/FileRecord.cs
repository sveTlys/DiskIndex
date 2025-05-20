using System;
namespace DiskIndex.Models { 
    public class FileRecord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Category { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public long? Size { get; set; }
        public DateTime LastModified { get; set; }
        public string IsNSFW { get; set; }  // "True", "False", "Unknown"

        // Trimming to hide the the root folder with the drive letter, useful especially in case the paths are long.
        public string TrimmedPath => Path?.Replace(@"F:\root\", "") ?? "";

        // Simple method to format the size of the files.
        public string FormattedSize
        {
            get
            {
                if (!Size.HasValue)
                    return "N/A";

                double size = Size.Value;

                if (size >= 1_073_741_824)
                    return $"{size / 1_073_741_824.0:F2} GB";
                else if (size >= 1_048_576)
                    return $"{size / 1_048_576.0:F2} MB";
                else if (size >= 1024)
                    return $"{size / 1024.0:F2} KB";
                else
                    return $"{size:F0} Bytes";
            }
        }
    }
}

﻿namespace MemoriesBackend.Domain.Models
{
    public class FileUploadedMetaData
    {
        public Guid Id { get; set; }
        public long Size { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
    }
}

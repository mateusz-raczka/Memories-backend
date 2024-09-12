namespace MemoriesBackend.Domain.Models.Storage
{
    public class FileUploadedResult
    {
        public Guid Id { get; set; }
        public long Size { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
    }
}

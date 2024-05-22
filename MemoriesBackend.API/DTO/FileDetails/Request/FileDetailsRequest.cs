namespace MemoriesBackend.API.DTO.FileDetails.Request
{
    public class FileDetailsRequest
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}

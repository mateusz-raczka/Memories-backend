namespace MemoriesBackend.API.DTO.FileDetails.Response
{
    public class FileDetailsResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Description { get; set; }
        public bool IsStared { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastOpenedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}

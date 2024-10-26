namespace MemoriesBackend.API.DTO.FileDetails.Request
{
    public class FileDetailsDescriptionPatchRequest
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}

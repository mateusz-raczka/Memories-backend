namespace MemoriesBackend.API.DTO.FileDetails.Request
{
    public class FileDetailsUpdateRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsStared { get; set; }
    }
}

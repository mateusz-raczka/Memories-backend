namespace MemoriesBackend.API.DTO.FileDetails.Request
{
    public class FileDetailsIsStaredPatchRequest
    {
        public Guid Id { get; set; }
        public bool IsStared { get; set; }
    }
}

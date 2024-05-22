using MemoriesBackend.API.DTO.FileDetails.Response;

namespace MemoriesBackend.API.DTO.File.Response
{
    public class FileCreateResponse
    {
        public Guid Id { get; set; }
        public Guid FolderId { get; set; }
        public FileDetailsResponse FileDetails { get; set; }
    }
}

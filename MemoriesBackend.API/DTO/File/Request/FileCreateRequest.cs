using MemoriesBackend.API.DTO.FileDetails.Request;

namespace MemoriesBackend.API.DTO.File.Request
{
    public class FileCreateRequest
    {
        public Guid Id { get; set; }
        public Guid FolderId { get; set; }
        public FileDetailsRequest FileDetails { get; set; }
    }
}

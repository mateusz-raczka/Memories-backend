using Memories_backend.Models.DTO.FileDetails.Request;

namespace Memories_backend.Models.DTO.File.Request
{
    public class FileDtoCreateRequest
    {
        public Guid Id { get; set; }
        public Guid FolderId { get; set; }
        public FileDetailsDtoRequest FileDetails { get; set; }
    }
}

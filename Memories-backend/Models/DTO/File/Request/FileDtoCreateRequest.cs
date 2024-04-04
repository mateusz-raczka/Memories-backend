using Memories_backend.Models.DTO.FileDetails.Request;

namespace Memories_backend.Models.DTO.File.Request
{
    public class FileDtoCreateRequest
    {
        public Guid? FolderId { get; set; }
        public IFormFile FileData { get; set; }
        public ComponentDetailsDtoRequest FileDetails { get; set; }
    }
}

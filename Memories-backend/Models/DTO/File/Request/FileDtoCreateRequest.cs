using Memories_backend.Models.DTO.FileDetails.Request;

namespace Memories_backend.Models.DTO.File.Request
{
    public class FileDtoCreateRequest
    {
        public Guid? FolderId { get; set; }
        public bool isFolder { get; set; }
        public Guid? CategoryId { get; set; }
        public FileDetailsDtoRequest FileDetails { get; set; }
    }
}

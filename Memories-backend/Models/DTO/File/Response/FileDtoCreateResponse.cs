using Memories_backend.Models.DTO.FileDetails.Response;

namespace Memories_backend.Models.DTO.File.Request
{
    public class FileDtoCreateResponse
    {
        public Guid Id { get; set; }
        public Guid? FolderId { get; set; }
        public bool isFolder { get; set; }
        public Guid? CategoryId { get; set; }
        public FileDetailsDtoResponse FileDetails { get; set; }
    }
}

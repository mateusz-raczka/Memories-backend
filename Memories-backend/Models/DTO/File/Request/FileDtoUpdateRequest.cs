using Memories_backend.Models.DTO.Category.Request;
using Memories_backend.Models.DTO.FileDetails.Request;
using Memories_backend.Models.DTO.Tag.Request;

namespace Memories_backend.Models.DTO.File.Request
{
    public class FileDtoUpdateRequest
    {
        public Guid? FolderId { get; set; }
        public List<TagDtoRequest>? Tags { get; set; }
        public CategoryDtoRequest? Category { get; set; }
        public ComponentDetailsDtoRequest? FileDetails { get; set; }
    }
}

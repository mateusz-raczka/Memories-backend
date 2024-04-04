using Memories_backend.Models.DTO.Category.Response;
using Memories_backend.Models.DTO.FileDetails.Response;
using Memories_backend.Models.DTO.Tag.Response;

namespace Memories_backend.Models.DTO.File.Response
{
    public class FileDtoFetchResponse
    {
        public Guid Id { get; set; }
        public Guid? FolderId { get; set; }
        public List<TagDtoResponse>? Tags { get; set; }
        public CategoryDtoResponse? Category { get; set; }
        public ComponentDetailsDtoResponse FileDetails { get; set; }
    }
}

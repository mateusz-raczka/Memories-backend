using MemoriesBackend.API.DTO.Category.Response;
using MemoriesBackend.API.DTO.FileDetails.Response;
using MemoriesBackend.API.DTO.Tag.Response;

namespace MemoriesBackend.API.DTO.File.Response
{
    public class FileGetByIdResponse
    {
        public Guid Id { get; set; }
        public Guid FolderId { get; set; }
        public List<TagResponse>? Tags { get; set; }
        public CategoryResponse? Category { get; set; }
        public FileDetailsResponse FileDetails { get; set; }
    }
}

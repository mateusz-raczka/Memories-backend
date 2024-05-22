using MemoriesBackend.API.DTO.Category.Request;
using MemoriesBackend.API.DTO.FileDetails.Request;
using MemoriesBackend.API.DTO.Tag.Request;

namespace MemoriesBackend.API.DTO.File.Request
{
    public class FileUpdateRequest
    {
        public Guid? FolderId { get; set; }
        public IEnumerable<TagRequest>? Tags { get; set; }
        public CategoryRequest? Category { get; set; }
        public FileDetailsRequest? FileDetails { get; set; }
    }
}

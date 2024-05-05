using Memories_backend.Models.DTO.FolderDetails.Request;

namespace Memories_backend.Models.DTO.Folder.Request
{
    public class FolderDtoCreateRequest
    {
        public Guid FolderId { get; set; }

        public FolderDetailsDtoRequest FolderDetails { get; set; }
    }
}

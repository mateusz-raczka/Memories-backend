using MemoriesBackend.API.DTO.FolderDetails.Request;

namespace MemoriesBackend.API.DTO.Folder.Request
{
    public class FolderCreateRequest
    {
        public Guid ParentFolderId { get; set; }

        public FolderDetailsRequest FolderDetails { get; set; }
    }
}

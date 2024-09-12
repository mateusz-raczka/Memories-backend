using MemoriesBackend.API.DTO.FolderDetails.Request;

namespace MemoriesBackend.API.DTO.Folder.Request
{
    public class FolderAddRequest
    {
        public Guid ParentFolderId { get; set; }

        public FolderDetailsRequest FolderDetails { get; set; }
    }
}

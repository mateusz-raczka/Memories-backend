using MemoriesBackend.API.DTO.File.Response;
using MemoriesBackend.API.DTO.FolderDetails.Response;

namespace MemoriesBackend.API.DTO.Folder.Response
{
    public class FolderCutAndPasteResponse
    {
        public Guid Id { get; set; }
        public Guid? ParentFolderId { get; set; }

        public FolderDetailsResponse FolderDetails { get; set; }
        public IEnumerable<FileGetAllResponse>? Files { get; set; }
        public IEnumerable<FolderGetAllResponse>? ChildFolders { get; set; }
    }
}

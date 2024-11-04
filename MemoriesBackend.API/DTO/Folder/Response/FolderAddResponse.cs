using MemoriesBackend.API.DTO.FolderDetails.Response;

namespace MemoriesBackend.API.DTO.Folder.Response
{
    public class FolderAddResponse
    {
        public Guid Id { get; set; }
        public Guid? ParentFolderId { get; set; }

        public FolderDetailsResponse FolderDetails { get; set; }
    }
}

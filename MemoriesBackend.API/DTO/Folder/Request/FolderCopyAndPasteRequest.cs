namespace MemoriesBackend.API.DTO.Folder.Request
{
    public class FolderCopyAndPasteRequest
    {
        public Guid SourceFolderId { get; set; }
        public Guid TargetFolderId { get; set; }
    }
}

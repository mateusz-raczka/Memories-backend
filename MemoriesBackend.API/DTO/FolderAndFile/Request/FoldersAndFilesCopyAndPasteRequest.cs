namespace MemoriesBackend.API.DTO.FolderAndFile.Request
{
    public class FoldersAndFilesCopyAndPasteRequest
    {
        public IEnumerable<Guid> FilesIds { get; set; }
        public IEnumerable<Guid> FoldersIds { get; set; }
        public Guid TargetFolderId { get; set; }
    }
}

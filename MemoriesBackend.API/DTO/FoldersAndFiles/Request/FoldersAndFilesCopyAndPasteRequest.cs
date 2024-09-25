namespace MemoriesBackend.API.DTO.FoldersAndFiles.Request
{
    public class FoldersAndFilesCopyAndPasteRequest
    {
        public IEnumerable<Guid> FilesIds { get; set; } = [];
        public IEnumerable<Guid> FoldersIds { get; set; } = [];
        public Guid TargetFolderId { get; set; }
    }
}

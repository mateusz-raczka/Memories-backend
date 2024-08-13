namespace MemoriesBackend.API.DTO.File.Request
{
    public class FileCopyPasteRequest
    {
        public IEnumerable<Guid> FilesId { get; set; }
        public Guid TargetFolderId { get; set; }
    }
}

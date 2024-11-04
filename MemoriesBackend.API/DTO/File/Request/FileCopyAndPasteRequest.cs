namespace MemoriesBackend.API.DTO.File.Request
{
    public class FileCopyAndPasteRequest
    {
        public IEnumerable<Guid> FilesIds { get; set; }
        public Guid TargetFolderId { get; set; }
    }
}

namespace MemoriesBackend.API.DTO.FoldersAndFiles.Request
{
    public class FoldersAndFilesDeleteRequest
    {
        public IEnumerable<Guid> FoldersIds { get; set; }
        public IEnumerable<Guid> FilesIds { get; set; }
    }
}

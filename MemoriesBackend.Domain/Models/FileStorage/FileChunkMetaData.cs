namespace MemoriesBackend.Domain.Models.FileStorage
{
    public class FileChunkMetaData
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public int ChunkIndex { get; set; }
        public int TotalChunks { get; set; }
    }
}

namespace MemoriesBackend.API.DTO.Folder.Response
{
    public class FolderGetByIdWithPathResponse
    {
        public FolderGetByIdResponse Folder { get; set; }
        public IEnumerable<FolderPathSegmentResposne> Path { get; set; } 
    }
}

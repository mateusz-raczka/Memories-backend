namespace MemoriesBackend.API.DTO.FolderDetails.Request
{
    public class FolderDetailsIsStaredPatchRequest
    {
        public Guid Id { get; set; }
        public bool isStared {  get; set; }
    }
}

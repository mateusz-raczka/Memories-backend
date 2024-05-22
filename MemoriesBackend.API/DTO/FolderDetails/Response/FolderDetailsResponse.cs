namespace MemoriesBackend.API.DTO.FolderDetails.Response
{
    public class FolderDetailsResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool? IsStared { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastOpenedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}

namespace Memories_backend.Models.DTO.FolderDetails.Response
{
    public class FolderDetailsDtoResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool? IsStared { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastOpenedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}

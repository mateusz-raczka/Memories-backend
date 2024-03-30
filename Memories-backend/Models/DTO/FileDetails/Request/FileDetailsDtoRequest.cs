namespace Memories_backend.Models.DTO.FileDetails.Request
{
    public class FileDetailsDtoRequest
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public string Description { get; set; }
        public bool IsStared { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastOpenedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}

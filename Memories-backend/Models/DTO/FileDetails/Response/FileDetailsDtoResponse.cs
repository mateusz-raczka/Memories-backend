using System.Text.Json.Serialization;

namespace Memories_backend.Models.DTO.FileDetails.Response
{
    public class FileDetailsDtoResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public bool IsStared { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastOpenedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}

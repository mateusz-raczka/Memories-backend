using System.Text.Json.Serialization;

namespace Memories_backend.Models.DTO.FileDetails.Request
{
    public class FileDetailsDtoRequest
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}

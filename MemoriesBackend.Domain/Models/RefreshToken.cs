namespace MemoriesBackend.Domain.Models
{
    public class RefreshToken
    {
        public string Value { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}

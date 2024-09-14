namespace MemoriesBackend.Domain.Models
{
    public class JwtToken
    {
        public string Value { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}

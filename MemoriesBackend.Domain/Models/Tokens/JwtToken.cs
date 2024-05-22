namespace MemoriesBackend.Domain.Models.Tokens
{
    public class JwtToken
    {
        public string Value { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}

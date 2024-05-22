namespace MemoriesBackend.Domain.Models.Tokens
{
    public class RefreshToken
    {
        public string Value { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}

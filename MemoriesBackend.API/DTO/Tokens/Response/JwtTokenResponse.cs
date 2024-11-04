namespace MemoriesBackend.API.DTO.Tokens.Response
{
    public class JwtTokenResponse
    {
        public string Value { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}

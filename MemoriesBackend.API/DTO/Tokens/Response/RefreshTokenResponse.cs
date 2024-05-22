namespace MemoriesBackend.API.DTO.Tokens.Response
{
    public class RefreshTokenResponse
    {
        public string Value { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}

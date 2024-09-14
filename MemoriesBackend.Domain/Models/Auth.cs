namespace MemoriesBackend.Domain.Models
{
    public class Auth
    {
        public UserData User { get; set; }
        public JwtToken AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}

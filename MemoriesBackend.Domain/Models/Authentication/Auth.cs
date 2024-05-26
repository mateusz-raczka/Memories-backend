using MemoriesBackend.Domain.Models.Tokens;
using MemoriesBackend.Domain.Models.User;

namespace MemoriesBackend.Domain.Models.Authentication
{
    public class Auth
    {
        public UserData User { get; set; }
        public JwtToken AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}

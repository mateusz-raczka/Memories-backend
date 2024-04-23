using Memories_backend.Models.Authentication;

namespace Memories_backend.Services.Interfaces
{
    public interface IUserClaimsService
    {
        UserClaimsValues UserClaimsValues { get; }
        void UpdateUserClaims(string token);
    }
}

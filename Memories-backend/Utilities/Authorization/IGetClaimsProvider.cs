namespace Memories_backend.Utilities.Authorization
{
    public interface IGetClaimsProvider
    {
        string UserId { get; }
        string UserName { get; }
        string Token { get; }
    }
}

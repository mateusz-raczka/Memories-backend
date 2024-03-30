namespace Memories_backend.Services
{
    public interface IUserClaimsService
    {
        Guid UserId { get; }
        string UserName { get; }
    }
}

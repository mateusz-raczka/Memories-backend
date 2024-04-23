namespace Memories_backend.Services.Interfaces
{
    public interface IInitializeUserService
    {
        Task InitializeUser(string token);
    }
}

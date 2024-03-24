namespace Memories_backend.Utilities.Authorization.DataAuthorize
{
    public interface IOwnerId
    {
        string OwnerId { get; }
        void SetOwnerId(string protectKey);
    }
}

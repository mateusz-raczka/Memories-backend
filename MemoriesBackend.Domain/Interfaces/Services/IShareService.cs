using MemoriesBackend.Domain.Entities;

namespace MemoriesBackend.Domain.Interfaces.Services;

public interface IShareService
{
    Task<ShareFile> ShareFileAsync(ShareFile shareFile);
}

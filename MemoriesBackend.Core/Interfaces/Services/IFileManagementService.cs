using Microsoft.AspNetCore.Http;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Application.Interfaces.Services
{
    public interface IFileManagementService
    {
        Task<File> AddFileToDatabaseAndStorageAsync(IFormFile fileData, Guid folderId);
    }
}

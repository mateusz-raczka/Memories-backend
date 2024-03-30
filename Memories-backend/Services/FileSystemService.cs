using Memories_backend.Utilities.Helpers;
using Microsoft.AspNetCore.StaticFiles;

namespace Memories_backend.Services
{
    public class FileSystemService : IFileSystemService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserClaimsService _userClaimsService;

        public FileSystemService(IConfiguration configuration, IUserClaimsService userClaimsService)
        {
            _configuration = configuration;
            _userClaimsService = userClaimsService;
        }

        public async Task UploadFileAsync(byte[] fileBytes, Guid id)
        {
            Guid userId = _userClaimsService.UserId;
            string directoryPath = GetDirectoryPath(userId);
            string filePath = GetFilePath(userId, id);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            await File.WriteAllBytesAsync(filePath, fileBytes);
        }

        public async Task<byte[]> DownloadFileAsync(Guid id)
        {
            Guid userId = _userClaimsService.UserId;
            string path = GetFilePath(userId, id);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File '{id}' not found.");
            }

            return await File.ReadAllBytesAsync(path);
        }

        public void DeleteFile(Guid id)
        {
            Guid userId = _userClaimsService.UserId;
            string path = GetFilePath(userId, id);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                throw new FileNotFoundException($"File '{id}' not found.");
            }
        }

        private string GetFilePath(Guid userId, Guid id)
        {
            return Path.Combine(_configuration["Storage:Path"], userId.ToString(), id.ToString());
        }

        private string GetDirectoryPath(Guid userId)
        {
            return Path.Combine(_configuration["Storage:Path"], userId.ToString());
        }
    }
}

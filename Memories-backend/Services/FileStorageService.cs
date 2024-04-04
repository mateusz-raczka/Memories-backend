namespace Memories_backend.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserClaimsService _userClaimsService;

        public FileStorageService(IConfiguration configuration, IUserClaimsService userClaimsService)
        {
            _configuration = configuration;
            _userClaimsService = userClaimsService;
        }

        public async Task<Guid> UploadFileAsync(IFormFile file, string folderHierarchy)
        {
            Guid id = Guid.NewGuid();
            Guid userId = _userClaimsService.UserId;
            string directoryPath = GetDirectoryPath(userId, folderHierarchy);
            string filePath = GetFilePath(userId, id, folderHierarchy);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return id;
        }

        public async Task<byte[]> DownloadFileAsync(Guid id, string path)
        {
            Guid userId = _userClaimsService.UserId;
            string filePath = GetFilePath(userId, id, path);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File '{id}' not found.");
            }

            return await File.ReadAllBytesAsync(filePath);
        }

        public void DeleteFile(Guid id, string folderHierarchy)
        {
            Guid userId = _userClaimsService.UserId;
            string filePath = GetFilePath(userId, id, folderHierarchy);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                throw new FileNotFoundException($"File '{id}' not found.");
            }
        }

        private string GetFilePath(Guid userId, Guid id, string folderHierarchy)
        {
            return Path.Combine(GetDirectoryPath(userId, folderHierarchy), id.ToString());
        }

        private string GetDirectoryPath(Guid userId, string folderHierarchy)
        {
            return Path.Combine(_configuration["Storage:Path"], userId.ToString(), folderHierarchy);
        }
        private IEnumerable<string> GetFolderHierarchy(Guid userId, string folderName)
        {
            List<string> hierarchy = new List<string>();

            string userFolderPath = Path.Combine(_configuration["Storage:Path"], userId.ToString());
            string currentFolderPath = Path.Combine(userFolderPath, folderName);
            while (!string.IsNullOrEmpty(currentFolderPath) && currentFolderPath != userFolderPath)
            {
                hierarchy.Insert(0, Path.GetFileName(currentFolderPath));

                currentFolderPath = Directory.GetParent(currentFolderPath)?.FullName;
            }

            if (currentFolderPath == userFolderPath)
            {
                hierarchy.Insert(0, Path.GetFileName(userFolderPath));
            }

            return hierarchy;
        }
    }
}

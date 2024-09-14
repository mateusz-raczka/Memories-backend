using MemoriesBackend.Application.Services;
using MemoriesBackend.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MemoriesBackend.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IFileDatabaseService, FileDatabaseService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IFileManagementService, FileManagementService>();
            services.AddScoped<IFolderDatabaseService, FolderDatabaseService>();
            services.AddScoped<IInitializeUserService, InitializeUserService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<IFolderStorageService, FolderStorageService>();
            services.AddScoped<ICookieService, CookieService>();
            services.AddScoped<IPathService, PathService>();
            services.AddScoped<IFolderManagementService, FolderManagementService>();
            services.AddScoped<IFolderAndFileManagementService, FolderAndFileManagementService>();

            return services;
        }
    }
}

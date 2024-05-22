using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Infrastructure.Contexts;
using MemoriesBackend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
        var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var dbUser = Environment.GetEnvironmentVariable("DB_USER");
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        var connectionString = $"Server={dbServer},{dbPort};Database={dbName};User Id={dbUser};Password={dbPassword};TrustServerCertificate=True;";
        //var connectionString = "Server=Mati\\MSSQLSERVER03;Database=memories;Trusted_Connection=True;TrustServerCertificate=True;";

        //Database context
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, options => { options.UseHierarchyId(); });
        });

        //Repositories
        services.AddScoped<IGenericRepository<File>, GenericRepository<File>>();
        services.AddScoped<IGenericRepository<FileDetails>, GenericRepository<FileDetails>>();
        services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
        services.AddScoped<IGenericRepository<Tag>, GenericRepository<Tag>>();
        services.AddScoped<IGenericRepository<FileActivity>, GenericRepository<FileActivity>>();
        services.AddScoped<IGenericRepository<Folder>, GenericRepository<Folder>>();
        services.AddScoped<IFolderRepository, FolderRepository>();

        return services;
    }
}
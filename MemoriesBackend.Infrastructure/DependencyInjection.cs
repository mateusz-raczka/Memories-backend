using MemoriesBackend.Domain.Entities;
using MemoriesBackend.Domain.Interfaces.Repositories;
using MemoriesBackend.Domain.Interfaces.Transactions;
using MemoriesBackend.Infrastructure.Contexts;
using MemoriesBackend.Infrastructure.Repositories;
using MemoriesBackend.Infrastructure.Transactions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using File = MemoriesBackend.Domain.Entities.File;

namespace MemoriesBackend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
        var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var dbUser = Environment.GetEnvironmentVariable("DB_USER");
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        var connectionString = $"Server={dbServer};Database={dbName};User Id={dbUser};Password={dbPassword};TrustServerCertificate=True;";

        //Database context
        services.AddDbContext<ApplicationDbContext>(options =>
        {
                    options.UseSqlServer(connectionString, options => { options.UseHierarchyId();
                    options.EnableRetryOnFailure(3);
                });
        });

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.User.RequireUniqueEmail = false;
            options.Password.RequireNonAlphanumeric = false;
            options.SignIn.RequireConfirmedEmail = false;
        });

        //Repositories
        services.AddScoped<IGenericRepository<File>, GenericRepository<File>>();
        services.AddScoped<IGenericRepository<FileDetails>, GenericRepository<FileDetails>>();
        services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
        services.AddScoped<IGenericRepository<Tag>, GenericRepository<Tag>>();
        services.AddScoped<IGenericRepository<FileActivity>, GenericRepository<FileActivity>>();
        services.AddScoped<IGenericRepository<Folder>, GenericRepository<Folder>>();
        services.AddScoped<IGenericRepository<FileUploadProgress>, GenericRepository<FileUploadProgress>>();

        //Transactions
        services.AddScoped<ITransactionHandler, TransactionHandler>();

        return services;
    }
}
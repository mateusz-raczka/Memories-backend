﻿global using Memories_backend.Middlewares;

using Memories_backend.Contexts;
using Memories_backend.Models.Domain;
using Memories_backend.Repositories;
using Memories_backend.Services;
using Memories_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using File = Memories_backend.Models.Domain.File;


namespace Memories_backend
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DevConnection");
            var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

            var connectionString = $"Server={dbServer},{dbPort};Database={dbName};User Id={dbUser};Password={dbPassword};TrustServerCertificate=True;";


            //Database context
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, options =>
                {
                    options.UseHierarchyId();
                });
            });

            //Authentication
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = false;
            });
       
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            //Repositories
            services.AddScoped<ISQLRepository<File>, SQLRepository<File>>();
            services.AddScoped<ISQLRepository<FileDetails>, SQLRepository<FileDetails>>();
            services.AddScoped<ISQLRepository<Category>, SQLRepository<Category>>();
            services.AddScoped<ISQLRepository<Tag>, SQLRepository<Tag>>();
            services.AddScoped<ISQLRepository<FileActivity>, SQLRepository<FileActivity>>();
            services.AddScoped<ISQLRepository<Folder>,  SQLRepository<Folder>>();
            services.AddScoped<IFolderRepository, FolderRepository>();
            
            //AutoMapper
            services.AddAutoMapper(typeof(Program));

            //Services
            services.AddScoped<IFileDatabaseService, FileDatabaseService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserClaimsService, UserClaimsService>();
            services.AddScoped<IJwtSecurityTokenService, JwtSecurityTokenService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IFileManagementService, FileManagementService>();
            services.AddScoped<IFolderDatabaseService, FolderDatabaseService>();
            services.AddScoped<IInitializeUserService, InitializeUserService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<IFolderStorageService, FolderStorageService>();

            //Middlewares that use other services
            services.AddScoped<JwtMiddleware>();
            services.AddScoped<GlobalExceptionHandlingMiddleware>();

            services.AddLogging();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:3000", "https://memories.maszaweb.pl")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            app.UseMiddleware<JwtMiddleware>();
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
        }
    }
}

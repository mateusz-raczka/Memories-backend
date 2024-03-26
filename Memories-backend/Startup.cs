global using Memories_backend.Middlewares;
global using Memories_backend.Utilities.Authorization;

using Memories_backend.Contexts;
using Memories_backend.Models.Domain;
using Memories_backend.Repositories;
using Memories_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            //Database context
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            //Repositories
            services.AddScoped<ISQLRepository<Models.Domain.File>, SQLRepository<Models.Domain.File>>();
            services.AddScoped<ISQLRepository<FileDetails>, SQLRepository<FileDetails>>();
            services.AddScoped<ISQLRepository<Category>, SQLRepository<Category>>();
            services.AddScoped<ISQLRepository<Tag>, SQLRepository<Tag>>();
            services.AddScoped<ISQLRepository<FileActivity>, SQLRepository<FileActivity>>();

            //AutoMapper
            services.AddAutoMapper(typeof(Program));

            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IGetClaimsProvider, GetClaimsFromUser>();
            services.AddScoped<JwtSecurityTokenHandlerWrapper>();
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
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            app.UseMiddleware<JwtMiddleware>();
            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

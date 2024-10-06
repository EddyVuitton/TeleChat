using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using TeleChat.Domain;
using TeleChat.WebAPI.Options.JWT;
using TeleChat.WebAPI.Repositories.Hub;
using TeleChat.WebAPI.Repositories.Account;
using TeleChat.WebAPI.Options.FilesContainer;
using TeleChat.WebAPI.Files;

namespace TeleChat.WebAPI.Extensions;

public static class BuilderExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddControllers().AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddLogging(options =>
        {
            options.AddConsole();
            options.AddDebug();
        });
        builder.Services.AddDbContextFactory<DBContext>(options =>
        {
            //options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection"));
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
        });
        builder.Services.AddScoped<IFileService, FileService>();
    }

    public static void AddHubChat(this WebApplicationBuilder builder)
    {
        var provider = builder.Services.BuildServiceProvider();
        var jwtOptions = provider.GetService<IOptions<JWTOptions>>()!;

        builder.Services.AddSignalR();
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuers = jwtOptions.Value.ValidIssuers,
                ValidAudiences = jwtOptions.Value.ValidAudiences,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key))
            };
        });
        builder.Services.AddAuthorization();
    }

    public static void AddOptions(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureOptions<JWTOptionsSetup>();
        builder.Services.ConfigureOptions<FilesContainerOptionsSetup>();
    }

    public static void AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IHubRepository, HubRepository>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
    }

    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<DBContext>();

        if (dbContext is not null)
        {
            await dbContext.Database.MigrateAsync();
        }
    }
}
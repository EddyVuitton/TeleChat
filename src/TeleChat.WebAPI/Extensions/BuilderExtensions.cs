﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using TeleChat.Domain;
using TeleChat.WebAPI.Account;
using TeleChat.WebAPI.Hub;
using TeleChat.WebAPI.Options.JWT;

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
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection"));
        });
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
    }

    public static void AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IHubRepository, HubRepository>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
    }

    public static async Task ReMigrateDatabaseAsync(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<DBContext>()!;

        context.Database.EnsureDeleted(); //zdropuj bazę danych, jeżeli istnieje...
        context.Database.Migrate(); //i stwórz kontekst bazy danych

        await context.AddSeedDataAsync();
    }
}
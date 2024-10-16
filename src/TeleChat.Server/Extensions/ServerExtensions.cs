﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using MudBlazor.Services;
using TeleChat.Server.Options.WebAPI;
using TeleChat.WebUI.Auth;
using TeleChat.WebUI.Services.App;
using TeleChat.WebUI.Services.Account;
using TeleChat.WebUI.Services.File;
using Microsoft.Extensions.FileProviders;

namespace TeleChat.Server.Extensions;

public static class ServerExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        var provider = builder.Services.BuildServiceProvider();
        var webapiOptions = provider.GetService<IOptions<WebAPIOptions>>()!;

        // Add services to the container.
        builder.Services
            .AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddCircuitOptions(o =>
            {
                o.DetailedErrors = true;
            });
        builder.Services.AddMudServices();
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(webapiOptions.Value.BaseAddress) });
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IAppService, AppService>();
        builder.Services.AddScoped<IFileService, FileService>();
    }

    public static void AddOptions(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureOptions<WebAPIOptionsSetup>();
    }

    public static void AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<JWTAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider, JWTAuthenticationStateProvider>(provider => provider.GetRequiredService<JWTAuthenticationStateProvider>());
        builder.Services.AddScoped<ILoginService, JWTAuthenticationStateProvider>(provider => provider.GetRequiredService<JWTAuthenticationStateProvider>());
    }

    public static void AddFileProvider(this WebApplication app)
    {
        var path = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "images");

        Directory.CreateDirectory(path);

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(path),
        });
    }
}
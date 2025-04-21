using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using MudBlazor.Services;
using TeleChat.Server.Options.Api;
using TeleChat.WebUI.Auth;
using TeleChat.ApiProxy.App;
using TeleChat.ApiProxy.Account;
using TeleChat.ApiProxy.Files;
using Microsoft.Extensions.FileProviders;

namespace TeleChat.Server.Extensions;

public static class ServerExtensions
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        var provider = builder.Services.BuildServiceProvider();
        var apiOptions = provider.GetService<IOptions<ApiOptions>>()!;

        // Add services to the container.
        builder.Services
            .AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddCircuitOptions(o =>
            {
                o.DetailedErrors = true;
            });
        builder.Services.AddMudServices();
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiOptions.Value.BaseAddress) });
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddScoped<IAppService, AppService>();
        builder.Services.AddScoped<IFileService, FileService>();
    }

    public static void AddOptions(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureOptions<ApiOptionsSetup>();
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
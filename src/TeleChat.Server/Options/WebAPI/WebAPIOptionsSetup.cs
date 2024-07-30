using Microsoft.Extensions.Options;

namespace TeleChat.Server.Options.WebAPI;

public class WebAPIOptionsSetup(IConfiguration configuration) : IConfigureOptions<WebAPIOptions>
{
    private const string _ConfigurationSectionName = "WebAPI";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(WebAPIOptions configuration)
    {
        _configuration.GetSection(_ConfigurationSectionName).Bind(configuration);
    }
}
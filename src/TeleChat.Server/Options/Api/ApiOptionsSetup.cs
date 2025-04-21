using Microsoft.Extensions.Options;

namespace TeleChat.Server.Options.Api;

public class ApiOptionsSetup(IConfiguration configuration) : IConfigureOptions<ApiOptions>
{
    private const string _ConfigurationSectionName = "Api";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(ApiOptions configuration)
    {
        _configuration.GetSection(_ConfigurationSectionName).Bind(configuration);
    }
}
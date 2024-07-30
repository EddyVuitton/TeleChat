using Microsoft.Extensions.Options;

namespace TeleChat.WebAPI.Options.JWT;

public class JWTOptionsSetup(IConfiguration configuration) : IConfigureOptions<JWTOptions>
{
    private const string _ConfigurationSectionName = "JWT";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(JWTOptions configuration)
    {
        _configuration.GetSection(_ConfigurationSectionName).Bind(configuration);
    }
}
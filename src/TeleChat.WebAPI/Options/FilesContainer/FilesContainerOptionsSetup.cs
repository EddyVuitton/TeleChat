using Microsoft.Extensions.Options;

namespace TeleChat.WebAPI.Options.FilesContainer;

public class FilesContainerOptionsSetup(IConfiguration configuration) : IConfigureOptions<FilesContainerOptions>
{
    private const string _ConfigurationSectionName = "FilesContainer";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(FilesContainerOptions configuration)
    {
        _configuration.GetSection(_ConfigurationSectionName).Bind(configuration);
    }
}
namespace TeleChat.WebAPI.Options.JWT;

public class JWTOptions
{
    public string[] ValidIssuers { get; set; } = [];
    public string[] ValidAudiences { get; set; } = [];
    public string Key { get; set; } = string.Empty;
}
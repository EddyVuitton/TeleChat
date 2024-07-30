using TeleChat.Domain.Auth;

namespace TeleChat.WebUI.Services.Main;

public interface IMainService
{
    Task<UserToken> BuildTokenAsync(string userName);
}
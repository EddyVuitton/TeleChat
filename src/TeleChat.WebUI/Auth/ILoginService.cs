using System.Security.Claims;
using TeleChat.Domain.Auth;

namespace TeleChat.WebUI.Auth;

public interface ILoginService
{
    Task LoginAsync(UserToken userToken);
    Task LogoutAsync();
    Task<(bool isLoggedIn, string? token, List<Claim>? userClaims)> IsLoggedInAsync();
    Task LogoutIfExpiredTokenAsync();
}
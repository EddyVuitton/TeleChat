using TeleChat.Domain.Models.Entities;

namespace TeleChat.Domain.Auth;

public class UserToken
{
    public User User { get; set; } = null!;
    public string Token { get; set; } = string.Empty;
}
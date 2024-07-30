using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeleChat.WebAPI.Hub;
using TeleChat.WebAPI.Options.JWT;

namespace TeleChat.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MainController(IHubContext<ChatHub> hubContext, IOptions<JWTOptions> options) : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext = hubContext;
    private readonly IOptions<JWTOptions> _options = options;

    [HttpPost("InfoAsync")]
    public async Task Index()
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "From WebApi", $"Home page loaded at: {DateTime.Now}");
    }

    [HttpPost("SendMessageAsync")]
    public async Task SendMessageAsync(string sentTo, string message, string fromUser)
    {
        await _hubContext.Clients.User(sentTo).SendAsync("ReceiveMessage", fromUser, message);
    }

    [HttpGet("BuildToken")]
    public UserToken BuildToken(string userName)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, userName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.Now.AddMinutes(1);

        JwtSecurityToken token = new(

            issuer: "TeleChat.WebUI",
            audience: "https://localhost:44362",
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new UserToken() { Token =  new JwtSecurityTokenHandler().WriteToken(token) };
    }
}

public class UserToken
{
    public string Token { get; set; } = string.Empty;
}
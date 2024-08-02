using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeleChat.Domain.Auth;
using TeleChat.WebAPI.Hubs;
using TeleChat.WebAPI.Options.JWT;

namespace TeleChat.WebAPI.Repositories
{
    public class MainRepository(IHubContext<ChatHub, IChatHub> hubContext, IOptions<JWTOptions> options)
    {
        private readonly IHubContext<ChatHub, IChatHub> _hubContext = hubContext;
        private readonly IOptions<JWTOptions> _options = options;

        public async Task AddToGroupAsync(string connectionId, string groupName)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, groupName);
        }

        public async Task SendToGroupAsync(string connectionId, string userName, string message, string groupName)
        {
            await _hubContext.Clients.GroupExcept(groupName, connectionId).ReceiveMessage(userName, message);
        }

        public UserToken BuildToken(string issuer, string audience, string userName)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, userName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.Now.AddMinutes(1);

            JwtSecurityToken token = new(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new UserToken() { Token =  new JwtSecurityTokenHandler().WriteToken(token) };
        }
    }
}
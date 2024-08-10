using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TeleChat.WebAPI.Hubs;

[Authorize]
public class ChatHub : Hub<IChatHub>
{

}
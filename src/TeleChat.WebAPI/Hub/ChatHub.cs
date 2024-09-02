using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TeleChat.WebAPI.Hub;

//[Authorize]
public class ChatHub : Hub<IChatHub>
{

}
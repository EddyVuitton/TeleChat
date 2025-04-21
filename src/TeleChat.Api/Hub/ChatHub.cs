using Microsoft.AspNetCore.SignalR;

namespace TeleChat.Api.Hub;

//[Authorize]
public class ChatHub : Hub<IChatHub>
{

}
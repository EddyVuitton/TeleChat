using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TeleChat.WebAPI.Hub;

[Authorize]
public class ChatHub : Hub<IChatHub>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.ReceiveMessage("ReceiveSystemMessage", $"{null ?? "xyz"} joined. ");
    }

    public async Task SendMessage(string sentTo, string message, string fromUser)
    {
        await Clients.Users(sentTo).ReceiveMessage(fromUser, message);
    }
}
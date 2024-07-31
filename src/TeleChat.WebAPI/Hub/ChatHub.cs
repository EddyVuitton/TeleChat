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

    public async Task JoinRoom(string roomName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
    }

    public async Task SendMessageToGroup(string message, string fromUser, string groupName)
    {
        await Clients.Group(groupName).ReceiveMessage(fromUser, message);
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TeleChat.WebAPI.Hubs;

[Authorize]
public class ChatHub : Hub<IChatHub>
{
    //public async Task AddToGroup(string connectionId, string groupName)
    //{
    //    await Groups.AddToGroupAsync(connectionId, groupName);
    //}

    //public async Task SendToGroup(string connectionId, string message, string groupName)
    //{
    //    await Clients.GroupExcept(groupName, connectionId).ReceiveMessage(connectionId, message);
    //}
}
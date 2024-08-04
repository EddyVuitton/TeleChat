using Microsoft.AspNetCore.SignalR;
using TeleChat.WebAPI.Hubs;

namespace TeleChat.WebAPI.Repositories
{
    public class MainRepository(IHubContext<ChatHub, IChatHub> hubContext)
    {
        private readonly IHubContext<ChatHub, IChatHub> _hubContext = hubContext;

        public async Task AddToGroupAsync(string connectionId, string groupName)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, groupName);
        }

        public async Task SendToGroupAsync(string connectionId, string userName, string message, string groupName)
        {
            await _hubContext.Clients.GroupExcept(groupName, connectionId).ReceiveMessage(userName, message);
        }
    }
}
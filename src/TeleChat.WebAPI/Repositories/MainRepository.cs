using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TeleChat.Domain.Context;
using TeleChat.Domain.Dtos;
using TeleChat.Domain.Entities;
using TeleChat.WebAPI.Hubs;

namespace TeleChat.WebAPI.Repositories
{
    public class MainRepository(IHubContext<ChatHub, IChatHub> hubContext, DBContext context)
    {
        private readonly IHubContext<ChatHub, IChatHub> _hubContext = hubContext;
        private readonly DBContext _context = context;

        public async Task AddToGroupAsync(string connectionId, string groupName)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, groupName);
        }

        public async Task SendToGroupAsync(string connectionId, string userName, string message, string groupName)
        {
            await _hubContext.Clients.GroupExcept(groupName, connectionId).ReceiveMessage(userName, message);
        }

        public async Task SendMessageAsync(MessageDto message)
        {
            message = message ?? throw new ArgumentNullException(nameof(message));
            
            var receiver = await _context.Receiver.FirstOrDefaultAsync(x => x.Id == message.Message.ReceiverId);
            
            if (receiver is not null && message.Message is not null)
            {
                await SendToGroupAsync(message.ConnectionId, message.User.Login, message.Message.Text, receiver.Name);
                var newMessage = new Message()
                {
                    Text = message.Message.Text,
                    TypeId = message.Message.TypeId,
                    ReceiverId = message.Message.ReceiverId,
                };

                await _context.Message.AddAsync(newMessage);
            }
        }
    }
}
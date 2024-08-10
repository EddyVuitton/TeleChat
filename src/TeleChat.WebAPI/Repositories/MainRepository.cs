﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data;
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

        #region Publics

        public async Task AddConnectionToGroupAsync(string connectionId, Guid groupChatGuid)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, groupChatGuid.ToString());
        }

        public async Task SendToGroupAsync(string connectionId, Message message)
        {
            await _hubContext.Clients.GroupExcept(message.GroupChat.Guid.ToString(), connectionId).ReceiveMessage(message);
        }

        public async Task<Message> SendMessageAsync(MessageDto message)
        {
            message = message ?? throw new ArgumentNullException(nameof(message));

            var newMessage = await AddMessageAsync(message) ?? throw new Exception("Could not add message to database..."); ;

            await SendToGroupAsync(message.ConnectionId, newMessage);

            return newMessage;
        }

        public async Task<List<MessageType>> GetMessageTypesAsync() => await _context.MessageType.AsNoTracking().ToListAsync();

        public async Task<List<UserGroupChat>> GetUserGroupChatsAsync(int userId)
        {
            var userGroupChats = await _context.UserGroupChat
                .Where(x => x.UserId == userId)
                .Include(x => x.GroupChat)
                .Include(x => x.User)
                .AsSplitQuery()
                .AsNoTracking()
                .ToListAsync();

            return userGroupChats;
        }

        #endregion

        #region Privates

        private async Task<Message?> AddMessageAsync(MessageDto message)
        {
            message = message ?? throw new ArgumentNullException(nameof(message));

            var groupChat = await _context.GroupChat.AsNoTracking().FirstOrDefaultAsync(x => x.Id == message.GroupChatId)
                ?? throw new NoNullAllowedException(nameof(message.GroupChatId));

            var messageType = await _context.MessageType.AsNoTracking().FirstOrDefaultAsync(x => x.Id == message.MessageTypeId)
                ?? throw new NoNullAllowedException(nameof(message.MessageTypeId));

            var user = await _context.User.AsNoTracking().FirstOrDefaultAsync(x => x.Id == message.UserId)
                ?? throw new NoNullAllowedException(nameof(message.UserId));

            var newMessage = new Message()
            {
                Text = message.Text,
                MessageType = messageType,
                User = user,
                GroupChat = groupChat
            };

            try
            {
                await _context.Message.AddAsync(newMessage);
                await _context.SaveChangesAsync();

                return newMessage;
            }
            catch
            {
                //to do...
            }

            return null;
        }

        #endregion
    }
}
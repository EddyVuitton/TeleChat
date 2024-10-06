using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TeleChat.Domain;
using TeleChat.Domain.Models.Entities;
using TeleChat.WebAPI.Hub;

namespace TeleChat.WebAPI.Repositories.Hub;

public class HubRepository(IHubContext<ChatHub, IChatHub> hubContext, DBContext context) : IHubRepository
{
    private readonly IHubContext<ChatHub, IChatHub> _hubContext = hubContext;
    private readonly DBContext _context = context;

    #region Publics

    public async Task AddConnectionToGroupAsync(string connectionId, Guid groupChatGuid)
    {
        await _hubContext.Groups.AddToGroupAsync(connectionId, groupChatGuid.ToString());
    }

    public async Task<Message> SendMessageAsync(MessageDto message)
    {
        message = message ?? throw new ArgumentNullException(nameof(message));

        var newMessage = await AddMessageAsync(message) ?? throw new Exception("Could not add message to database...");

        await SendToGroupAsync(message.ConnectionId, newMessage);

        return newMessage;
    }

    public async Task<List<MessageType>> GetMessageTypesAsync()
    {
        return await _context.MessageType.AsNoTracking().ToListAsync();
    }

    public async Task<List<UserGroupChat>> GetUserGroupChatsAsync(int userId)
    {
        var userGroupChats = await _context.UserGroupChat
            .Where(x => x.UserId == userId)
            .Include(x => x.GroupChat)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        return userGroupChats;
    }

    public async Task<GroupChat?> GetDefaultGroupChatAsync()
    {
        return await _context.GroupChat.AsNoTracking().SingleOrDefaultAsync(x => x.Id == 1);
    }

    public async Task<List<Message>> GetGroupChatMessagesAsync(int groupChatId)
    {
        var messages = await _context.Message
            .Where(x => x.GroupChatId == groupChatId)
            .Include(x => x.User)
            .Include(x => x.MessageType)
            .AsNoTracking()
            .ToListAsync();

        return messages;
    }

    public async Task<UserGroupChat> AddGroupChatAsync(GroupChatDto groupChat)
    {
        groupChat = groupChat ?? throw new ArgumentNullException(nameof(groupChat));
        var user = await _context.User.SingleAsync(x => x.Id == groupChat.User.Id)
            ?? throw new ArgumentException($"Nie istnieje user o id: {groupChat.User.Id}");

        if (string.IsNullOrEmpty(groupChat.Name))
        {
            throw new ArgumentException($"Nazwa [{nameof(groupChat.Name)}] nie może być pusta");
        }

        var newGroupChat = new GroupChat()
        {
            Name = groupChat.Name
        };

        var newUserGroupChat = new UserGroupChat()
        {
            User = user,
            GroupChat = newGroupChat,
        };

        await _context.AddAsync(newGroupChat);        
        await _context.AddAsync(newUserGroupChat);

        await _context.SaveChangesAsync();

        return new();
    }

    public async Task DeleteGroupChatAsync(int groupChatId)
    {
        var userGroupChats = _context.UserGroupChat.Where(x => x.GroupChatId == groupChatId);
        var messages = _context.Message.Where(x => x.GroupChatId == groupChatId);
        var groupChat = await _context.GroupChat.FirstOrDefaultAsync(x => x.Id == groupChatId);

        if (groupChat is not null)
        {
            _context.RemoveRange(userGroupChats);
            _context.RemoveRange(messages);
            _context.Remove(groupChat);

            await _context.SaveChangesAsync();
        }
    }

    #endregion

    #region Privates

    private async Task<Message?> AddMessageAsync(MessageDto message)
    {
        message = message ?? throw new ArgumentNullException(nameof(message));

        var groupChat = await _context.GroupChat.FirstOrDefaultAsync(x => x.Id == message.GroupChatId)
            ?? throw new NoNullAllowedException(nameof(message.GroupChatId));

        var messageType = await _context.MessageType.FirstOrDefaultAsync(x => x.Id == message.MessageTypeId)
            ?? throw new NoNullAllowedException(nameof(message.MessageTypeId));

        var user = await _context.User.FirstOrDefaultAsync(x => x.Name == message.UserName)
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
            await _context.AddAsync(newMessage);
            await _context.SaveChangesAsync();

            return newMessage;
        }
        catch
        {
            //to do...
        }

        return null;
    }

    private async Task SendToGroupAsync(string connectionId, Message message)
    {
        if (message.GroupChat is not null)
        {
            await _hubContext.Clients.GroupExcept(message.GroupChat.Guid.ToString(), connectionId).ReceiveMessage(message);
        }
    }

    #endregion
}
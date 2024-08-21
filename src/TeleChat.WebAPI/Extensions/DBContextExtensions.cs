using TeleChat.Domain.Context;
using TeleChat.Domain.Models.Entities;
using TeleChat.WebAPI.Helpers;

namespace TeleChat.WebAPI.Extensions;

public static class DBContextExtensions
{
    public static async Task AddSeedDataAsync(this DBContext context)
    {
        var user = new User()
        {
            Name = "Demo",
            Login = "demo",
            Password = AuthHelper.HashPassword("demo"),
            IsActive = true
        };

        var messageTypes = new List<MessageType>
        {
            new() { Name = "PlainText" },
            new() { Name = "GIF", DefaultStyle = "max-width: 200px; max-height: 200px;" }
        };

        var groupChat = new GroupChat()
        {
            Name = "Domyślna grupa"
        };

        var userGroupChat = new UserGroupChat()
        {
            User = user,
            GroupChat = groupChat
        };

        await context.User.AddAsync(user);
        await context.MessageType.AddRangeAsync(messageTypes);
        await context.GroupChat.AddAsync(groupChat);
        await context.UserGroupChat.AddAsync(userGroupChat);

        await context.SaveChangesAsync();
    }
}
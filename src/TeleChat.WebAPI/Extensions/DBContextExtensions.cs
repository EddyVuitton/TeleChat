using TeleChat.Domain.Context;
using TeleChat.Domain.Entities;

namespace TeleChat.WebAPI.Extensions;

public static class DBContextExtensions
{
    public static async Task AddSeedDataAsync(this DBContext context)
    {
        var receiverTypes = new List<ReceiverType>
        {
            new() { Name = "UserChat" },
            new() { Name = "GroupChat" }
        };

        await context.ReceiverType.AddRangeAsync(receiverTypes);

        var messageTypes = new List<MessageType>
        {
            new() { Name = "PlainText", DefaultStyle = string.Empty },
            new() { Name = "GIFs", DefaultStyle = "max-width: 200px; max-height: 200px;" }
        };

        await context.MessageType.AddRangeAsync(messageTypes);

        await context.SaveChangesAsync();
    }
}
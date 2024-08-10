using TeleChat.Domain.Entities;

namespace TeleChat.WebAPI.Hubs;

public interface IChatHub
{
    Task ReceiveMessage(Message message);
}
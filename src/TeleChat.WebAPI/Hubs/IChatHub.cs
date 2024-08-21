using TeleChat.Domain.Models.Entities;

namespace TeleChat.WebAPI.Hubs;

public interface IChatHub
{
    Task ReceiveMessage(Message message);
}
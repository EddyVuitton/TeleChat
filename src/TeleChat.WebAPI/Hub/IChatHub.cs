using TeleChat.Domain.Models.Entities;

namespace TeleChat.WebAPI.Hub;

public interface IChatHub
{
    Task ReceiveMessage(Message message);
}
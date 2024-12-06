using TeleChat.Domain.Models.Entities;
using TeleChat.Domain;

namespace TeleChat.WebAPI.Hub;

public interface IChatHub
{
    Task ReceiveMessage(Message message);
    Task RefreshMessageReactions(ReactionDto reaction);
}
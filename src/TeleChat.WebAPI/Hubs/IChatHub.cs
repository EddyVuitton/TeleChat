namespace TeleChat.WebAPI.Hubs;

public interface IChatHub
{
    Task ReceiveMessage(string user, string message);
    //Task AddToGroup(string connectionId, string groupName);
    //Task SendToGroup(string connectionId, string message, string groupName);
}
namespace TeleChat.WebAPI.Hub;

public interface IChatHub
{
    Task ReceiveMessage(string user, string message);
}
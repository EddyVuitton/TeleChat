namespace TeleChat.WebUI.Services.Main;

public class MainService(HttpClient httpClient) : IMainService
{
    private readonly HttpClient _httpClient = httpClient;
    private const string _MainRoute = "api/Main";

    #region Publics

    public async Task AddToGroupAsync(string connectionId, string groupName)
    {
        var parameters = new List<string>
        {
            $"connectionId={connectionId}",
            $"groupName={groupName}"
        };

        string queryString = string.Join("&", parameters);
        string url = $"{_MainRoute}/AddToGroupAsync?{queryString}";

        var response = await _httpClient.PostAsync(url, null);
        response.EnsureSuccessStatusCode();
    }

    public async Task SendToGroupAsync(string connectionId, string userName, string message, string groupName)
    {
        var parameters = new List<string>
        {
            $"connectionId={connectionId}",
            $"userName={userName}",
            $"message={message}",
            $"groupName={groupName}"
        };

        string queryString = string.Join("&", parameters);
        string url = $"{_MainRoute}/SendToGroupAsync?{queryString}";

        var response = await _httpClient.PostAsync(url, null);
        response.EnsureSuccessStatusCode();
    }

    #endregion
}
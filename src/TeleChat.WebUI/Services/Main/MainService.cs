using Newtonsoft.Json;
using System.Reflection;
using TeleChat.Domain.Auth;

namespace TeleChat.WebUI.Services.Main;

public class MainService(HttpClient httpClient) : IMainService
{
    private readonly HttpClient httpClient = httpClient;
    private const string _MainRoute = "api/Main";

    public async Task<UserToken> BuildTokenAsync(string userName)
    {
        var issuer = Assembly.GetAssembly(typeof(MainService))?.GetName().Name;
        var audience = httpClient.BaseAddress;

        var parameters = new List<string>
        {
            $"issuer={issuer}",
            $"audience={audience}",
            $"userName={userName}"
        };

        string queryString = string.Join("&", parameters);
        string url = $"{_MainRoute}/BuildToken?{queryString}";

        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<UserToken>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task AddToGroupAsync(string connectionId, string groupName)
    {
        var parameters = new List<string>
        {
            $"connectionId={connectionId}",
            $"groupName={groupName}"
        };

        string queryString = string.Join("&", parameters);
        string url = $"{_MainRoute}/AddToGroupAsync?{queryString}";

        var response = await httpClient.PostAsync(url, null);
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

        var response = await httpClient.PostAsync(url, null);
        response.EnsureSuccessStatusCode();
    }
}
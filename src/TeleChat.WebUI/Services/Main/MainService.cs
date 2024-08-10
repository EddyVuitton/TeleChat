using Newtonsoft.Json;
using System.Text;
using TeleChat.Domain.Dtos;
using TeleChat.Domain.Entities;

namespace TeleChat.WebUI.Services.Main;

public class MainService(HttpClient httpClient) : IMainService
{
    private readonly HttpClient _httpClient = httpClient;
    private const string _MainRoute = "api/Main";

    #region Publics

    public async Task AddConnectionToGroupAsync(string connectionId, Guid groupChatGuid)
    {
        var parameters = new List<string>
        {
            $"connectionId={connectionId}",
            $"groupChatGuid={groupChatGuid}"
        };

        string queryString = string.Join("&", parameters);
        string url = $"{_MainRoute}/AddConnectionToGroupAsync?{queryString}";

        var response = await _httpClient.PostAsync(url, null);
        response.EnsureSuccessStatusCode();
    }

    public async Task<Message?> SendMessageAsync(MessageDto message)
    {
        var json = JsonConvert.SerializeObject(message);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_MainRoute}/SendMessageAsync", content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<Message>(responseContent);

        return deserialisedResponse;
    }

    public async Task<List<MessageType>> GetMessageTypesAsync()
    {
        string url = $"{_MainRoute}/GetMessageTypesAsync";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<List<MessageType>>(responseContent) ?? [];

        return deserialisedResponse;
    }

    public async Task<List<UserGroupChat>> GetUserGroupChatsAsync(int userId)
    {
        string url = $"{_MainRoute}/GetUserGroupChatsAsync?userId={userId}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<List<UserGroupChat>>(responseContent) ?? [];

        return deserialisedResponse;
    }

    #endregion
}
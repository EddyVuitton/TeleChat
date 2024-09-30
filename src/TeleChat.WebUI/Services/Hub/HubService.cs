using Microsoft.AspNetCore.SignalR.Client;
using TeleChat.Domain.Models.Entities;
using TeleChat.Domain;
using Newtonsoft.Json;
using System.Text;
using Microsoft.JSInterop;
using TeleChat.WebUI.Extensions;

namespace TeleChat.WebUI.Services.Hub;

public class HubService(HttpClient httpClient, IJSRuntime js) : IHubService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IJSRuntime _js = js;
    private const string _HubRoute = "api/Hub";

    public HubConnection CreateHubConnection(string token)
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:44362/Chat", options =>
            {
                options.AccessTokenProvider = async () => await Task.FromResult(token);
            })
            .Build();

        return hubConnection;
    }

    public async Task AddConnectionToGroupAsync(string connectionId, Guid groupChatGuid)
    {
        var parameters = new List<string>
        {
            $"connectionId={connectionId}",
            $"groupChatGuid={groupChatGuid}"
        };

        string queryString = string.Join("&", parameters);
        string url = $"{_HubRoute}/AddConnectionToGroupAsync?{queryString}";

        var response = await _httpClient.PostAsync(url, null);
        response.EnsureSuccessStatusCode();
    }

    public async Task<Message?> SendMessageAsync(MessageDto message)
    {
        var json = JsonConvert.SerializeObject(message);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_HubRoute}/SendMessageAsync", content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<Message>(responseContent);

        return deserialisedResponse;
    }

    public async Task<List<MessageType>> GetMessageTypesAsync()
    {
        string url = $"{_HubRoute}/GetMessageTypesAsync";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<List<MessageType>>(responseContent) ?? [];

        return deserialisedResponse;
    }

    public async Task<List<UserGroupChat>> GetUserGroupChatsAsync(int userId)
    {
        try
        {
            string url = $"{_HubRoute}/GetUserGroupChatsAsync?userId={userId}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var deserialisedResponse = JsonConvert.DeserializeObject<List<UserGroupChat>>(responseContent) ?? [];

            return deserialisedResponse;
        }
        catch (Exception ex)
        {
            await _js.LogAsync(ex);
            return [];
        }
    }

    public async Task<GroupChat?> GetDefaultGroupChatAsync()
    {
        try
        {
            string url = $"{_HubRoute}/GetDefaultGroupChat";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var deserialisedResponse = JsonConvert.DeserializeObject<GroupChat>(responseContent);

            return deserialisedResponse;
        }
        catch (Exception ex)
        {
            await _js.LogAsync(ex);
            return null;
        }
    }

    public async Task<List<Message>> GetGroupChatMessagesAsync(int groupChatId)
    {
        try
        {
            string url = $"{_HubRoute}/GetGroupChatMessages?groupChatId={groupChatId}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var deserialisedResponse = JsonConvert.DeserializeObject<List<Message>>(responseContent) ?? [];

            return deserialisedResponse;
        }
        catch (Exception ex)
        {
            await _js.LogAsync(ex);
            return [];
        }
    }

    public async Task<UserGroupChat?> AddGroupChatAsync(GroupChatDto groupChat)
    {
        try
        {
            var json = JsonConvert.SerializeObject(groupChat);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_HubRoute}/AddGroupChatAsync", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var deserialisedResponse = JsonConvert.DeserializeObject<UserGroupChat>(responseContent);

            return deserialisedResponse;
        }
        catch (Exception ex)
        {
            await _js.LogAsync(ex);
            return null;
        }
    }

    public async Task DeleteGroupChatAsync(int groupChatId)
    {
        try
        {
            string url = $"{_HubRoute}/DeleteGroupChat?groupChatId={groupChatId}";
            var response = await _httpClient.PostAsync(url, null);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            await _js.LogAsync(ex);
        }
    }
}
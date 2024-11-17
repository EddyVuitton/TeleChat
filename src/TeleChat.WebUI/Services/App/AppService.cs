using Microsoft.AspNetCore.SignalR.Client;
using TeleChat.Domain.Models.Entities;
using TeleChat.Domain;
using Newtonsoft.Json;
using System.Text;
using Microsoft.JSInterop;
using TeleChat.WebUI.Extensions;

namespace TeleChat.WebUI.Services.App;

public class AppService(HttpClient httpClient, IJSRuntime js) : IAppService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IJSRuntime _js = js;
    private const string _AppRoute = "api/App";

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
        string url = $"{_AppRoute}/AddConnectionToGroup?{queryString}";

        var response = await _httpClient.PostAsync(url, null);
        response.EnsureSuccessStatusCode();
    }

    public async Task<Message?> SendMessageAsync(MessageDto message)
    {
        var json = JsonConvert.SerializeObject(message);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_AppRoute}/SendMessage", content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<Message>(responseContent);

        return deserialisedResponse;
    }

    public async Task<List<MessageType>> GetMessageTypesAsync()
    {
        string url = $"{_AppRoute}/GetMessageTypes";
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
            string url = $"{_AppRoute}/GetUserGroupChats?userId={userId}";
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
            string url = $"{_AppRoute}/GetDefaultGroupChat";
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
            string url = $"{_AppRoute}/GetGroupChatMessages?groupChatId={groupChatId}";
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

            var response = await _httpClient.PostAsync($"{_AppRoute}/AddGroupChat", content);
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
            string url = $"{_AppRoute}/DeleteGroupChat?groupChatId={groupChatId}";
            var response = await _httpClient.PostAsync(url, null);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            await _js.LogAsync(ex);
        }
    }

    public async Task DeleteMessageAsync(int messageId)
    {
        try
        {
            string url = $"{_AppRoute}/DeleteMessage?messageId={messageId}";
            var response = await _httpClient.PostAsync(url, null);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            await _js.LogAsync(ex);
        }
    }

    public async Task<List<Reaction>> GetReactionsAsync()
    {
        try
        {
            string url = $"{_AppRoute}/GetReactions";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var deserialisedResponse = JsonConvert.DeserializeObject<List<Reaction>>(responseContent) ?? [];

            return deserialisedResponse;
        }
        catch (Exception ex)
        {
            await _js.LogAsync(ex);
            return [];
        }
    }

    public async Task<MessageReaction?> AddReactionAsync(ReactionDto reaction)
    {
        try
        {
            var json = JsonConvert.SerializeObject(reaction);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_AppRoute}/AddReaction", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var deserialisedResponse = JsonConvert.DeserializeObject<MessageReaction>(responseContent);

            return deserialisedResponse;
        }
        catch (Exception ex)
        {
            await _js.LogAsync(ex);
            return null;
        }
    }

    public async Task<List<ReactionDto>> GetChatReactionsAsync(int chatId)
    {
        try
        {
            string url = $"{_AppRoute}/GetChatReactions?chatId={chatId}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var deserialisedResponse = JsonConvert.DeserializeObject<List<ReactionDto>>(responseContent) ?? [];

            return deserialisedResponse;
        }
        catch (Exception ex)
        {
            await _js.LogAsync(ex);
            return [];
        }
    }
}
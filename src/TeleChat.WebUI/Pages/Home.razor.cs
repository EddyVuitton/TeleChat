using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using TeleChat.Domain.Models.Entities;
using TeleChat.Domain.Extensions;
using TeleChat.Domain;
using TeleChat.WebUI.Components.Chat;
using TeleChat.WebUI.Hub;
using TeleChat.WebUI.Account;

namespace TeleChat.WebUI.Pages;

public partial class Home : IAsyncDisposable
{
    [Inject] public IJSRuntime JS { get; set; } = null!;
    [Inject] public IAccountService AccountService { get; init; } = null!;
    [Inject] public IHubService HubService { get; set; } = null!;

    private HubConnection? hubConnection;

    private string _newMessageInput = string.Empty;
    private string _groupName = string.Empty;
    private string _userName = string.Empty;
    private string _connectionId = string.Empty;
    private string _token = string.Empty;

    private List<MessageType> _messageTypes = [];
    private User? _user;
    private GroupChat _groupChat = new();
    private readonly List<Message> _messages = [];

    private ChatBox _chatBox = new();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _messageTypes = await HubService.GetMessageTypesAsync();
            _token = (await AccountService.GetTokenAsync()).Token;
            _groupChat = await HubService.GetDefaultGroupChatAsync();
        }
        catch (Exception ex)
        {
            await JS.LogAsync(ex);
        }
    }

    #region Publics

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }

    public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

    #endregion

    #region Privates

    private void AddMessage(Message message)
    {
        _messages.Add(message);
    }

    private async Task Connect()
    {
        try
        {
            if ((hubConnection is null || hubConnection.ConnectionId is null) && !string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_groupName))
            {
                _user = await AccountService.CreateUser(_userName);

                hubConnection = HubService.CreateHubConnection(_token);

                if (hubConnection is null)
                {
                    return;
                }

                hubConnection.On<Message>("ReceiveMessage", (message) =>
                {
                    AddMessage(message);
                    InvokeAsync(_chatBox.RefreshChatAsync);
                });
                
                await hubConnection.StartAsync();

                _connectionId = hubConnection.ConnectionId ?? string.Empty;

                if (!string.IsNullOrEmpty(_connectionId))
                {
                    await HubService.AddConnectionToGroupAsync(_connectionId, _groupChat.Guid);
                    await AccountService.CreateUser(_userName);
                }
            }
        }
        catch (Exception ex)
        {
            await JS.LogAsync(ex);
        }
    }

    private async Task SendMessageAsync()
    {
        try
        {
            if (!string.IsNullOrEmpty(_newMessageInput))
            {
                var messageType = _messageTypes.First(x => x.Name == "PlainText");

                var messageDto = new MessageDto(
                    _newMessageInput,
                    _connectionId,
                    1,
                    _user.Id,
                    1,
                    _userName
                );

                var sentMessage = await HubService.SendMessageAsync(messageDto);

                if (sentMessage is not null)
                {
                    AddMessage(sentMessage);
                    _newMessageInput = new(string.Empty);
                    await _chatBox.RefreshChatAsync();
                }
            }
        }
        catch (Exception ex)
        {
            await JS.LogAsync(ex);
        }
    }

    #endregion
}
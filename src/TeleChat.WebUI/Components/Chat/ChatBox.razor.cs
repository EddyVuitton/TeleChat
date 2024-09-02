using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using TeleChat.Domain;
using TeleChat.Domain.Extensions;
using TeleChat.Domain.Models.Entities;
using TeleChat.WebUI.Services.Hub;

namespace TeleChat.WebUI.Components.Chat;

public partial class ChatBox : IAsyncDisposable
{
    [Inject] public IJSRuntime JS { get; private init; } = null!;
    [Inject] public IScrollManager ScrollManager { get; private init; } = null!;
    [Inject] public IHubService HubService { get; private init; } = null!;

    [Parameter] public List<Message> Messages { get; set; } = [];
    [Parameter] public User? User { get; set; }

    public GroupChat? GroupChat { get; set; }

    private HubConnection? _hubConnection;
    private string? _connectionId;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("initializeChat", DotNetObjectReference.Create(this));
            await RefreshChatAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }

        GC.SuppressFinalize(this);
    }

    [JSInvokable]
    public async Task LoadOlderMessages()
    {
        return;

        if (Messages.Count > 0)
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    var message = Messages[0];
                    message.Text = "dwadioawdjioawdwadioawdj";
                    message.UserId = -1;
                    message.User = new User() { Name = "Application" };
                    message.Created = Messages.Min(x => x.Created).AddSeconds(-1);

                    AddMessage(message);
                }
            });
            StateHasChanged();
        }
    }

    public async Task RefreshChatAsync()
    {
        StateHasChanged();
        await Task.Delay(10);
        await ScrollManager.ScrollToBottomAsync("#chat", ScrollBehavior.Smooth);
    }

    private void AddMessage(Message message)
    {
        Messages.Add(message);        
    }

    public async Task<string> ConnectAsync(GroupChat groupChat)
    {
        GroupChat = groupChat;

        if (_hubConnection is null)
        {
            try
            {
                _hubConnection = HubService.CreateHubConnection(string.Empty);

                _hubConnection.On<Message>("ReceiveMessage", (message) =>
                {
                    AddMessage(message);
                    InvokeAsync(RefreshChatAsync);
                });

                await _hubConnection.StartAsync();

                if (_hubConnection is not null && _hubConnection.ConnectionId is not null)
                {
                    _connectionId = _hubConnection.ConnectionId;
                    await HubService.AddConnectionToGroupAsync(_connectionId, GroupChat.Guid);
                }

                StateHasChanged();
                return _connectionId ?? string.Empty;
            }
            catch (Exception ex)
            {
                await JS.LogAsync(ex);
            }
        }

        return _connectionId ?? string.Empty;
    }

    public async Task<bool> SendMessageAsync(string messageText)
    {
        try
        {
            if (!string.IsNullOrEmpty(messageText) && !string.IsNullOrEmpty(_connectionId) && User is not null && GroupChat is not null)
            {
                var messageDto = new MessageDto(
                    messageText,
                    _connectionId,
                    1,
                    User.Id,
                    GroupChat.Id,
                    User.Name
                );

                var sentMessage = await HubService.SendMessageAsync(messageDto);

                if (sentMessage is not null)
                {
                    AddMessage(sentMessage);
                    await RefreshChatAsync();
                }

                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            await JS.LogAsync(ex);
            return false;
        }
    }
}
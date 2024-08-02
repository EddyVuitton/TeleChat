using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using TeleChat.Domain.Dtos;
using TeleChat.WebUI.Services.Main;

namespace TeleChat.WebUI.Pages;

public partial class Home : IAsyncDisposable
{
    [Inject] public IJSRuntime JS { get; set; } = null!;
    [Inject] public IMainService Service { get; set; } = null!;
    [Inject] public IScrollManager ScrollManager { get; set; } = null!;

    private HubConnection? hubConnection;

    private readonly List<UserMessageDto> _messages = [];
    private const int _MessagesAmountToLoad = 5;
    private string _newMessageInput = string.Empty;
    private string _groupName = string.Empty;
    private string _userName = string.Empty;
    private string _connectionId = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("initializeChat", DotNetObjectReference.Create(this));
            await Task.Delay(10);
            await ScrollManager.ScrollToBottomAsync("#chat", ScrollBehavior.Smooth);

            StateHasChanged();
        }
    }

    #region Publics

    [JSInvokable]
    public async Task LoadOlderMessages()
    {
        if (_messages.Count > 0)
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < _MessagesAmountToLoad; i++)
                {
                    AddMessage(_messages, _userName, false, "123");
                }
            });
            StateHasChanged();
        }
    }

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

    private void AddMessage(List<UserMessageDto> list, string from, bool isNew, string message)
    {
        var newId = (_messages?.Count > 0 ? _messages.Max(x => x.Id) : 0) + 1;
        newId = isNew ? -(_messages?.Count) ?? -1 : newId;

        list.Add(new UserMessageDto(newId, from, new MessageDto(message, DateTime.Now)));
    }

    private async Task Connect()
    {
        if (hubConnection is null && !string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_groupName))
        {
            try
            {
                var token = (await Service.BuildTokenAsync(_userName)).Token;

                hubConnection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:44362/Chat", options =>
                    {
                        options.AccessTokenProvider = async () => await Task.FromResult(token);
                    })
                    .Build();

                hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    AddMessage(_messages, user, true, message);
                    InvokeAsync(RefreshChat);
                });

                await hubConnection.StartAsync();

                if (!string.IsNullOrEmpty(hubConnection.ConnectionId))
                {
                    _connectionId = hubConnection.ConnectionId;
                    await Service.AddToGroupAsync(_connectionId, _groupName);
                }

                StateHasChanged();
            }
            catch
            {

            }
        }
    }

    private async Task SendToGroup()
    {
        if (hubConnection is not null && !string.IsNullOrEmpty(_newMessageInput))
        {
            await Service.SendToGroupAsync(_connectionId, _userName, _newMessageInput, _groupName);

            AddMessage(_messages, _userName, true, _newMessageInput);

            _newMessageInput = new(string.Empty);

            await RefreshChat();
        }
    }

    private async Task RefreshChat()
    {
        StateHasChanged();
        await Task.Delay(10);
        await ScrollManager.ScrollToBottomAsync("#chat", ScrollBehavior.Smooth);
    }

    #endregion
}
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using TeleChat.Domain.Dtos;
using TeleChat.Domain.Entities;
using TeleChat.Domain.Extensions;
using TeleChat.WebUI.Auth;
using TeleChat.WebUI.Dialogs.Auth;
using TeleChat.WebUI.Services.Account;
using TeleChat.WebUI.Services.Main;

namespace TeleChat.WebUI.Pages;

public partial class Home : IAsyncDisposable
{
    [Inject] public IJSRuntime JS { get; set; } = null!;
    [Inject] public IMainService MainService { get; set; } = null!;
    [Inject] public IScrollManager ScrollManager { get; set; } = null!;
    [Inject] public ILoginService LoginService { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; init; } = null!;
    [Inject] public IAccountService AccountService { get; init; } = null!;

    private HubConnection? hubConnection;

    private const int _MessagesAmountToLoad = 5;
    private string _newMessageInput = string.Empty;
    private string _groupName = string.Empty;
    private string _userName = string.Empty;
    private string _connectionId = string.Empty;
    private string _token = string.Empty;

    private List<MessageType> _messageTypes = [];
    private User _user = new();
    private GroupChat _groupChat = new();
    private List<UserGroupChat> _userGroupChats = [];
    private readonly List<Message> _messages = [];
    private bool _isLoggedIn = false;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _messageTypes = await MainService.GetMessageTypesAsync();
        }
        catch (Exception ex)
        {
            await JS.LogAsync(ex);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            //await LoginService.LogoutAsync();

            await LoginService.LogoutIfExpiredTokenAsync();

            var (isLoggedIn, token, claims) = await LoginService.IsLoggedInAsync();
            if (isLoggedIn && token is not null)
            {
                _token = token;
                //var userLogin = claims?.FirstOrDefault(x => x.Type == "UserLogin");

                //if (userLogin is not null)
                //{
                //    try
                //    {
                //        var user = await AccountService.GetUserByLoginAsync(userLogin.Value);

                //        if (user is not null)
                //        {
                //            _user = user;
                //        }

                //        var userGroupChats = await MainService.GetUserGroupChatsAsync(_user.Id);

                //        if (userGroupChats.Count > 0)
                //        {
                //            _userGroupChats = userGroupChats;
                //            _groupChat = _userGroupChats.First().GroupChat;
                //            _groupName = _groupChat.Guid.ToString();
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        await JS.LogAsync(ex);
                //    }
                //}
            }

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
                    var message = _messages[0];
                    message.Text = "dwadioawdjioawdwadioawdjioawdwadioawdjioawdwadioawdjioawdwadi";
                    message.UserId = -1;
                    message.User.Name = "Application";
                    message.Created = _messages.Min(x => x.Created).AddSeconds(-1);

                    AddMessage(message);
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

    public async Task SetIsLoggedIn(bool value, string login = "")
    {
        _isLoggedIn = value;

        if (_isLoggedIn)
        {
            _groupChat = (await MainService.GetUserGroupChatsAsync(1))[0].GroupChat;
            _groupName = _groupChat.Name;

            _user = (await AccountService.GetUserByLoginAsync(login))!;
            _userName = _user.Name;

            var (isLoggedIn, token, claims) = await LoginService.IsLoggedInAsync();

            _token = token;

            if (_token is null)
            {
                await JS.LogAsync("_token is null");
            }
        }

        StateHasChanged();
    }

    #endregion

    #region Privates

    private void AddMessage(Message message)
    {
        _messages.Add(message);
    }

    private async Task Login()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var parameters = new DialogParameters
        {
            { "HomePage", this }
        };

        await DialogService.ShowAsync<LoginDialog>(null, parameters, options);
    }

    private async Task Register()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        await DialogService.ShowAsync<RegisterDialog>(null, options);
    }

    private async Task LogOut()
    {
        await LoginService.LogoutAsync();
        StateHasChanged();
    }

    private async Task Connect()
    {
        try
        {
            if (hubConnection is null && !string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_groupName))
            {
                hubConnection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:44362/Chat", options =>
                    {
                        options.AccessTokenProvider = async () => await Task.FromResult(_token);
                    })
                    .Build();

                hubConnection.On<Message>("ReceiveMessage", (message) =>
                {
                    AddMessage(message);
                    InvokeAsync(RefreshChat);
                });

                //hubConnection.On<Message>("ReceiveMessage", MessageHandler);
                
                await hubConnection.StartAsync();

                if (!string.IsNullOrEmpty(hubConnection.ConnectionId))
                {
                    _connectionId = hubConnection.ConnectionId;

                    await MainService.AddConnectionToGroupAsync(_connectionId, _groupChat.Guid);
                }

                StateHasChanged();
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
                    _groupChat.Id
                );

                var sentMessage = await MainService.SendMessageAsync(messageDto);

                if (sentMessage is not null)
                {
                    AddMessage(sentMessage);
                    _newMessageInput = new(string.Empty);
                    await RefreshChat();
                }
            }
        }
        catch (Exception ex)
        {
            await JS.LogAsync(ex);
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
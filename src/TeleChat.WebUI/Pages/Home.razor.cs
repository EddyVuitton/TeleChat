using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using TeleChat.WebUI.Extensions;
using TeleChat.Domain.Models.Entities;
using TeleChat.WebUI.Components.Chat;
using TeleChat.WebUI.Dialogs.Auth;
using TeleChat.WebUI.Services.App;
using TeleChat.Domain.Auth;
using TeleChat.WebUI.Components.Sidebar.Menu;
using Microsoft.AspNetCore.Components.Forms;

namespace TeleChat.WebUI.Pages;

public partial class Home
{
    #region DependencyInjection

    [Inject] public IDialogService DialogService { get; private set; } = null!;
    [Inject] public IAppService AppService { get; private init; } = null!;
    [Inject] public IJSRuntime JS { get; private init; } = null!;

    #endregion

    #region Fields

    private UserToken? _userToken;
    private ChatBox? _selectedChatBox;
    private SidebarMenu? _refSidebarMenu;

    private readonly List<GroupChat> _groups = [];
    private readonly List<int> _contacts = [];
    private List<Message> _messages = [];

    private bool _isUserDataLoaded = false;
    private bool _isChatInitialized = false;
    private string _selectedGroupChatName = string.Empty;
    private string _newMessageText = string.Empty;

    #endregion

    #region Properties

    public UserToken? UserToken
    {
        set
        {
            _userToken = value;
            StateHasChanged();
            _refSidebarMenu?.InvokeOnInitialized();
            _refSidebarMenu?.InvokeStateHasChanged();
        }
    }

    #endregion

    #region LifecycleEvents

    protected override async Task OnInitializedAsync()
    {
        if (_userToken is null)
        {
            await OpenLoginDialog();
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_isUserDataLoaded && _userToken is not null)
        {
            await LoadGroupsAndContactsAsync(_userToken);
        }
    }

    #endregion

    #region PublicMethods

    public async Task LoadGroupChat(GroupChat groupChat)
    {
        if (groupChat is null)
        {
            return;
        }

        if (!_isUserDataLoaded || _userToken is null)
        {
            return;
        }

        if (_isChatInitialized && _selectedChatBox is not null && _selectedChatBox.GroupChat is not null)
        {
            await DisconnectChatAsync(_selectedChatBox.GroupChat.Id);
        }

        _isChatInitialized = false;
        StateHasChanged();

        var messages = await AppService.GetGroupChatMessagesAsync(groupChat.Id);
        _messages.Clear();
        _messages.AddRange(messages);
        _selectedGroupChatName = groupChat.Name;

        if (_selectedChatBox is not null)
        {
            await _selectedChatBox.DisposeAsync();
            var connectionId = await _selectedChatBox.ConnectAsync(groupChat, _userToken.Token);

            if (!string.IsNullOrEmpty(connectionId))
            {
                _isChatInitialized = true;
                await _selectedChatBox.RefreshChatAsync();
            }

            await JS.LogAsync($"Połączono {groupChat.Name} ConnectionID: {connectionId}");
        }

        StateHasChanged();
    }

    public async Task InvokeLoadGroupsAndContactsAsync()
    {
        if (_userToken is not null)
        {
            await LoadGroupsAndContactsAsync(_userToken);
        }
    }

    public async Task DisconnectChatAsync(int groupChatId)
    {
        if (_selectedChatBox is not null && _selectedChatBox.GroupChat?.Id == groupChatId)
        {
            await _selectedChatBox.DisposeAsync();
            _isChatInitialized = false;
            _selectedGroupChatName = string.Empty;
            _messages = [];

            await JS.LogAsync($"Rozłączono {_selectedChatBox.GroupChat.Name}");

            StateHasChanged();
        }
    }

    #endregion

    #region PrivateMethods

    private async Task OpenLoginDialog()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = false,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            BackdropClick = false
        };

        var parameters = new DialogParameters
        {
            { "HomePage", this }
        };

        await DialogService.ShowAsync<LoginDialog>(string.Empty, parameters, options);
    }

    private async Task SendMessageAsync()
    {
        if (string.IsNullOrEmpty(_newMessageText))
        {
            return;
        }

        if (_selectedChatBox is null)
        {
            return;
        }

        var result = await _selectedChatBox.SendMessageAsync(_newMessageText);

        if (result)
        {
            _newMessageText = string.Empty;
        }
    }

    private async Task LoadGroupsAndContactsAsync(UserToken userToken)
    {
        var userGroupChats = await AppService.GetUserGroupChatsAsync(userToken.User.Id);
        _groups.Clear();
        _contacts.Clear();

        foreach (var ugc in userGroupChats)
        {
            if (ugc.GroupChat is not null)
            {
                _groups.Add(ugc.GroupChat);
            }
        }
        _contacts.Add(1);

        _isUserDataLoaded = true;
        StateHasChanged();
    }

    private async void SendFile(IBrowserFile file)
    {
        if (_selectedChatBox is null)
        {
            return;
        }

        await _selectedChatBox.SendFileAsync(file);
    }

    #endregion
}
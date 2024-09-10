using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using TeleChat.WebUI.Extensions;
using TeleChat.Domain.Models.Entities;
using TeleChat.WebUI.Components.Chat;
using TeleChat.WebUI.Dialogs.Auth;
using TeleChat.WebUI.Services.Hub;
using TeleChat.Domain.Auth;
using Microsoft.AspNetCore.Components.Web;
using TeleChat.WebUI.Components.Sidebar.Menu;

namespace TeleChat.WebUI.Pages;

public partial class Home
{
    #region DependencyInjection

    [Inject] public IDialogService DialogService { get; private set; } = null!;
    [Inject] public IHubService HubService { get; private init; } = null!;
    [Inject] public IJSRuntime JS { get; private init; } = null!;

    #endregion

    #region Fields

    private UserToken? _userToken;
    private ChatBox? _selectedChatBox;
    private SidebarMenu? _refSidebarMenu;

    private readonly List<GroupChat> _groups = [];
    private readonly List<int> _contacts = [];
    private readonly List<Message> _messages = [];

    private bool _isUserDataLoaded = false;
    private bool _isChatInitialized = false;
    private bool _isSidebardMenuPopoverOpen = false;
    private string _selectedGroupChatName = string.Empty;
    private string _newMessageText = string.Empty;
    private string _popoverStyle = string.Empty;

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
        _isChatInitialized = false;
        StateHasChanged();

        if (groupChat is null)
        {
            return;
        }

        if (!_isUserDataLoaded || _userToken is null)
        {
            return;
        }

        var messages = await HubService.GetGroupChatMessagesAsync(groupChat.Id);
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

            await JS.LogAsync($"{groupChat.Name} ConnectionID: {connectionId}");
        }

        StateHasChanged();
    }

    public void ClosePopoverMenu()
    {
        _isSidebardMenuPopoverOpen = false;
        _popoverStyle = string.Empty;
        StateHasChanged();
    }

    public async Task InvokeLoadGroupsAndContactsAsync()
    {
        if (_userToken is not null)
        {
            await LoadGroupsAndContactsAsync(_userToken);
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
        var userGroupChats = await HubService.GetUserGroupChatsAsync(userToken.User.Id);
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

    #endregion
}
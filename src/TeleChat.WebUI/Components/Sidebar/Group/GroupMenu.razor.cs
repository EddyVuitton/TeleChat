using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TeleChat.WebUI.Services.Hub;

namespace TeleChat.WebUI.Components.Sidebar.Group;

public partial class GroupMenu
{
    #region DependencyInjection

    [Inject] public IHubService HubService { get; private init; } = null!;

    #endregion

    #region Fields

    private bool _isOpenPopoverMenu;
    private string _popoverStyle = string.Empty;

    #endregion

    #region Properties

    [Parameter] public GroupItem? GroupItem { get; init; }

    #endregion

    #region PublicMethods

    public void TogglePopoverMenu(MouseEventArgs args)
    {
        if (_isOpenPopoverMenu)
        {
            ClosePopoverMenu();
        }
        else
        {
            OpenPopoverMenu(args);
        }
    }

    #endregion

    #region PrivateMethods

    private void ClosePopoverMenu()
    {
        _isOpenPopoverMenu = false;
        _popoverStyle = string.Empty;
        StateHasChanged();
    }

    private void OpenPopoverMenu(MouseEventArgs args)
    {
        SetPopoverStyle(args);
        _isOpenPopoverMenu = true;

        StateHasChanged();
    }

    private void SetPopoverStyle(MouseEventArgs args)
    {
        var clientX = args?.ClientX.ToString("0.##");
        var clientY = args?.ClientY.ToString("0.##");
        _popoverStyle = $"padding: 10px 0px; position: fixed !important; left: {clientX}px; top: {clientY}px;";
    }

    private async Task DeleteGroupChat()
    {
        if (GroupItem is not null && GroupItem.Item is not null)
        {
            await HubService.DeleteGroupChatAsync(GroupItem.Item.Id);

            if (GroupItem.HomePage is not null)
            {
                await GroupItem.HomePage.InvokeLoadGroupsAndContactsAsync();
                await GroupItem.HomePage.DisconnectChatOnGroupDelete(GroupItem.Item.Id);
            }
        }
    }

    #endregion
}
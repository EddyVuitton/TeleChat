using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TeleChat.WebUI.Customs;
using TeleChat.WebUI.Services.App;

namespace TeleChat.WebUI.Components.Sidebar.Group;

public partial class GroupMenu
{
    #region DependencyInjection

    [Inject] public IAppService AppService { get; private init; } = null!;

    #endregion

    #region Properties

    [Parameter] public GroupItem? GroupItem { get; init; }

    private TCPopover? _popoverRef { get; set; }

    #endregion

    #region PublicMethods

    public void TogglePopoverMenu(MouseEventArgs args)
    {
        _popoverRef?.TogglePopoverMenu(args);
    }

    #endregion

    #region PrivateMethods

    private async Task DeleteGroupChat()
    {
        if (GroupItem is not null && GroupItem.Item is not null)
        {
            await AppService.DeleteGroupChatAsync(GroupItem.Item.Id);

            if (GroupItem.HomePage is not null)
            {
                await GroupItem.HomePage.InvokeLoadGroupsAndContactsAsync();
                await GroupItem.HomePage.DisconnectChatAsync(GroupItem.Item.Id);
            }
        }
    }

    #endregion
}
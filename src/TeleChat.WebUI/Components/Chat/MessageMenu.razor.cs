using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using TeleChat.WebUI.Customs;
using TeleChat.Domain.Models.Entities;

namespace TeleChat.WebUI.Components.Chat;

public partial class MessageMenu
{
    #region Properties

    [Parameter] public Message Message { get; init; } = null!;
    [Parameter] public ChatBox ChatBox { get; init; } = null!;

    private TCPopover? _popoverRef { get; set; }

    #endregion

    #region PublicMethods

    public void TogglePopoverMenu(MouseEventArgs args)
    {
        _popoverRef?.TogglePopoverMenu(args);
    }

    #endregion

    #region PrivateMethods

    private void DeleteClick()
    {
        ChatBox.RemoveMessage(Message);
        _popoverRef?.TogglePopoverMenu(null);
    }

    #endregion
}
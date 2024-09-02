using Microsoft.AspNetCore.Components;
using TeleChat.Domain.Models.Entities;

namespace TeleChat.WebUI.Components.Chat;

public partial class ChatMessage
{
    #region Fields

    private bool _isLeft;
    private string _timeStamp = string.Empty;
    private string _class = string.Empty;
    private string _timeStampStyle = string.Empty;

    #endregion

    #region Properties

    [Parameter] public Message? Message { get; init; }
    [Parameter] public User? LoggedUser { get; init; }

    #endregion

    #region LifecycleEvents

    protected override void OnInitialized()
    {
        if (Message is null || LoggedUser is null)
        {
            return;
        }

        _isLeft = Message.User?.Id != LoggedUser.Id;
        _timeStamp = Message.Created.ToString("HH:mm");
        _class = "message " + (_isLeft ? "message-left" : "message-right");
        _timeStampStyle = "color: " + (_isLeft ? "#959595" : "#bbb0ee");
    }

    #endregion
}
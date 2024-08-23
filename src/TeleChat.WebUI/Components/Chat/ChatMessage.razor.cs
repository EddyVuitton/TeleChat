using Microsoft.AspNetCore.Components;

namespace TeleChat.WebUI.Components.Chat;

public partial class ChatMessage
{
    [Parameter] public bool IsLeft { get; set; }
    [Parameter] public string? UserName { get; set; }
    [Parameter] public string Text { get; set; } = string.Empty;
    [Parameter] public string TimeStamp { get; set; } = string.Empty;

    private string _class = string.Empty;
    private string _timeStampStyle = string.Empty;

    protected override void OnInitialized()
    {
        _class = "message " + (IsLeft ? "message-left" : "message-right");
        _timeStampStyle = "color: " + (IsLeft ? "#959595" : "#bbb0ee");
    }
}
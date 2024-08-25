using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using TeleChat.Domain.Models.Entities;

namespace TeleChat.WebUI.Components.Chat;

public partial class ChatBox
{
    [Inject] public IJSRuntime JS { get; private init; } = null!;
    [Inject] public IScrollManager ScrollManager { get; private init; } = null!;

    [Parameter] public List<Message> Messages { get; set; } = [];
    [Parameter] public User User { get; set; } = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JS.InvokeVoidAsync("initializeChat", DotNetObjectReference.Create(this));
            await RefreshChatAsync();
        }
    }

    [JSInvokable]
    public async Task LoadOlderMessages()
    {
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
}
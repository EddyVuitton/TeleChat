using Microsoft.AspNetCore.Components;
using MudBlazor;
using TeleChat.WebUI.Auth;
using TeleChat.WebUI.Dialogs.Auth;

namespace TeleChat.WebUI.Components.Account;

public partial class AppBarOptions
{
    [Inject] public ILoginService LoginService { get; init; } = null!;
    [Inject] public IDialogService DialogService { get; init; } = null!;
    [Inject] public NavigationManager NavigationManager { get; init; } = null!;

    private async Task LogOut()
    {
        await LoginService.LogoutAsync();
        NavigationManager.NavigateTo("/", true);
    }

    private async Task OpenLoginDialog()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        await DialogService.ShowAsync<LoginDialog>(string.Empty, options);
    }
}
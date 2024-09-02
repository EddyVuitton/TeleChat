using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using TeleChat.WebUI.Extensions;
using TeleChat.Domain.Forms;
using TeleChat.WebUI.Services.Account;

namespace TeleChat.WebUI.Dialogs.Auth;

public partial class RegisterDialog
{
    #region DependencyInjection

    [Inject] public IAccountService AccountService { get; private init; } = null!;
    [Inject] public IDialogService DialogService { get; private init; } = null!;
    [Inject] public IJSRuntime JS { get; private init; } = null!;

    #endregion

    #region Fields

    private readonly RegisterAccountForm _model = new();

    #endregion

    #region Properties

    [CascadingParameter] public MudDialogInstance MudDialog { get; private init; } = null!;

    #endregion

    #region PrivateMethods

    private async void OnValidSubmit(EditContext context)
    {
        try
        {
            var registerForm = context.Model as RegisterAccountForm ?? new();
            await AccountService.RegisterAsync(registerForm);

            await OpenLoginDialog();
        }
        catch (Exception ex)
        {
            await JS.LogAsync(ex);
        }
    }

    private async Task OpenLoginDialog()
    {
        Cancel();
        await Task.Delay(400);

        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var parameters = new DialogParameters
        {
            { "RegisterAccountForm", _model }
        };

        DialogService.Show<LoginDialog>(null, parameters, options);
    }

    private void Cancel() => MudDialog.Cancel();

    #endregion
}
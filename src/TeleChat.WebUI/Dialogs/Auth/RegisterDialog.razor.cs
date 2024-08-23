using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using TeleChat.Domain.Extensions;
using TeleChat.Domain.Forms;
using TeleChat.WebUI.Account;

namespace TeleChat.WebUI.Dialogs.Auth;

public partial class RegisterDialog
{
    [Inject] public IAccountService AccountService { get; init; } = null!;
    [Inject] public IDialogService DialogService { get; init; } = null!;
    [Inject] public IJSRuntime JS { get; set; } = null!;

    [CascadingParameter] public MudDialogInstance MudDialog { get; private init; } = null!;

    private readonly RegisterAccountForm _model = new();

    private async void OnValidSubmit(EditContext context)
    {
        try
        {
            var registerForm = context.Model as RegisterAccountForm ?? new();
            await AccountService.RegisterAsync(registerForm);

            //SnackbarService.Show("Konto zostało poprawnie zarejestrowane", Severity.Success, true, false);
            await OpenLoginDialog();
        }
        catch (Exception ex)
        {
            //SnackbarService.Show("Konto nie zostało zarejestrowane", Severity.Warning, true, false);
            //SnackbarService.Show(ex.Message, Severity.Error);

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
}
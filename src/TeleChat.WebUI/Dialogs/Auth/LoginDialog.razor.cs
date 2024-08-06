using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using TeleChat.Domain.Extensions;
using TeleChat.Domain.Forms;
using TeleChat.WebUI.Auth;
using TeleChat.WebUI.Services.Account;

namespace TeleChat.WebUI.Dialogs.Auth;

public partial class LoginDialog
{
    [Inject] public IAccountService AccountService { get; init; } = null!;
    [Inject] public ILoginService LoginService { get; init; } = null!;
    [Inject] public IDialogService DialogService { get; init; } = null!;
    [Inject] public IJSRuntime JS { get; set; } = null!;

    [CascadingParameter] public MudDialogInstance MudDialog { get; private init; } = null!;

    [Parameter] public RegisterAccountForm? RegisterAccountForm { get; set; }

    private readonly LoginAccountForm _model = new();

    protected override void OnInitialized()
    {
        if (RegisterAccountForm is not null)
        {
            _model.Login = RegisterAccountForm.Login;
            _model.Password = string.Empty;
        }
    }

    private async void OnValidSubmit(EditContext context)
    {
        try
        {
            var loginForm = context.Model as LoginAccountForm ?? new();
            var response = await AccountService.LoginAsync(loginForm);

            await LoginService.LoginAsync(response);
            Cancel();
        }
        catch (Exception ex)
        {
            //SnackbarService.Show(ex.Message, Severity.Error, true, false);
            await JS.LogAsync(ex);
        }
    }

    private void Cancel() => MudDialog.Cancel();

    private async Task OpenRegisterDialog()
    {
        Cancel();
        await Task.Delay(250);

        var options = new DialogOptions
        {
            CloseOnEscapeKey = false,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        DialogService.Show<RegisterDialog>(null, options);
    }
}
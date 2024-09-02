using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using TeleChat.WebUI.Extensions;
using TeleChat.Domain.Forms;
using TeleChat.WebUI.Pages;
//using TeleChat.WebUI.Auth;
using TeleChat.WebUI.Services.Account;

namespace TeleChat.WebUI.Dialogs.Auth;

public partial class LoginDialog
{
    #region DependencyInjection

    [Inject] public IAccountService AccountService { get; private init; } = null!;
    //[Inject] public ILoginService LoginService { get; private init; } = null!;
    [Inject] public IDialogService DialogService { get; private init; } = null!;
    [Inject] public IJSRuntime JS { get; private init; } = null!;

    #endregion

    #region Fields

    private readonly LoginAccountForm _model = new();

    #endregion

    #region Properties

    [CascadingParameter] public MudDialogInstance MudDialog { get; private init; } = null!;

    [Parameter] public RegisterAccountForm? RegisterAccountForm { get; set; }
    [Parameter] public Home? HomePage { get; set; }

    #endregion

    #region LifecycleEvents

    protected override void OnInitialized()
    {
        _model.Login = "demo";
        _model.Password = "demo";

        if (RegisterAccountForm is not null)
        {
            _model.Login = RegisterAccountForm.Login;
            _model.Password = string.Empty;
        }
    }

    #endregion

    #region PrivateMethods

    private async void OnValidSubmit(EditContext context)
    {
        try
        {
            //Tymczasowo na potrzeby rozwoju aplikacji
            var loginForm = context.Model as LoginAccountForm ?? new();
            var user = await AccountService.CreateUser(loginForm.Login);
            var response = await AccountService.LoginAsync(loginForm);

            if (HomePage is not null)
            {
                HomePage.LoggedUser = user;
            }

            //await LoginService.LoginAsync(response);

            Cancel();
        }
        catch (Exception ex)
        {
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
            CloseOnEscapeKey = true,
            NoHeader = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        DialogService.Show<RegisterDialog>(null, options);
    }

    #endregion
}
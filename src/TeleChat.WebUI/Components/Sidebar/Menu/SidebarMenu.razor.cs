using Microsoft.AspNetCore.Components;
using MudBlazor;
using TeleChat.Domain.Models.Entities;
using TeleChat.WebUI.Dialogs.Menu;
using TeleChat.WebUI.Pages;

namespace TeleChat.WebUI.Components.Sidebar.Menu;

public partial class SidebarMenu
{
    #region DependencyInjection

    [Inject] public IDialogService DialogService { get; private init; } = null!;

    #endregion

    #region Fields

    private bool _isUserNull = false;

    #endregion

    #region Properties

    [Parameter] public User? User { get; init; }
    [Parameter] public Home? HomePage { get; init; }

    #endregion

    #region LifecycleEvents

    protected override void OnInitialized()
    {
        _isUserNull = User is null;
    }

    #endregion

    #region PublicMethods

    public void InvokeStateHasChanged() => StateHasChanged();

    public void InvokeOnInitialized() => OnInitialized();

    #endregion

    #region PrivateMethods

    private async Task OpenAddGroupDialog()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            NoHeader = true,
            FullWidth = true,
            CloseButton = true
        };

        var parameters = new DialogParameters
        {
            { "User", User },
            { "HomePage", HomePage }
        };

        await DialogService.ShowAsync<AddGroupDialog>(string.Empty, parameters, options);
    }

    #endregion
}
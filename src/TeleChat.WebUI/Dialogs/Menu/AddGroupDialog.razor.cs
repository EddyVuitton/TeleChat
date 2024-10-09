using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using TeleChat.Domain;
using TeleChat.Domain.Forms;
using TeleChat.Domain.Models.Entities;
using TeleChat.WebUI.Extensions;
using TeleChat.WebUI.Pages;
using TeleChat.WebUI.Services.App;

namespace TeleChat.WebUI.Dialogs.Menu;

public partial class AddGroupDialog
{
    #region DependencyInjection

    [Inject] public IAppService AppService { get; private init; } = null!;
    [Inject] public IJSRuntime JS { get; private init; } = null!;

    #endregion

    #region Properties

    [CascadingParameter] public MudDialogInstance MudDialog { get; private init; } = null!;

    [Parameter] public User? User { get; init; }
    [Parameter] public List<User> Contacts { get; init; } = [];
    [Parameter] public Home? HomePage { get; init; }

    #endregion

    #region Fields

    private readonly AddGroupForm _groupFormModel = new();
    private MudCarousel<object>? _refMudCarousel;
    private GroupChatDto? _groupChatDto;
    private IEnumerable<User> _selectedMembers = [];

    #endregion

    #region PrivateMethods

    private void OnValidSubmitGroupForm()
    {
        if (User is not null)
        {
            _groupChatDto = new GroupChatDto(_groupFormModel.Name, User, null);
            _refMudCarousel?.Next();
        }
    }

    private async Task OnValidSubmitMembers()
    {
        try
        {
            if (User is not null && _groupChatDto is not null)
            {
                var response = await AppService.AddGroupChatAsync(_groupChatDto);
                if (HomePage is not null)
                {
                    await HomePage.InvokeLoadGroupsAndContactsAsync();
                }
            }
        }
        catch (Exception ex)
        {
            await JS.LogAsync(ex);
        }
        finally
        {
            Cancel();
        }
    }

    private void Cancel() => MudDialog.Cancel();

    #endregion
}
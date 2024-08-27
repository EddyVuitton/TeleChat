using TeleChat.Domain.Models.Entities;

namespace TeleChat.WebUI.Layout;

public partial class MainLayout
{
    //Tymczasowo na potrzeby rozwoju aplikacji
    private User? _loggedUser;

    public User? User
    {
        get => _loggedUser;
        set
        {
            _loggedUser = value;
            StateHasChanged();
        }
    }
}
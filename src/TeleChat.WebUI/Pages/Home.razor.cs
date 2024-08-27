using Microsoft.AspNetCore.Components;
using TeleChat.WebUI.Layout;

namespace TeleChat.WebUI.Pages;

public partial class Home
{
    [CascadingParameter] public MainLayout MainLayout { get; set; } = null!;

    private readonly List<int> _groups = Enumerable.Range(1, 1).ToList();
    private readonly List<int> _contacts = Enumerable.Range(1, 1).ToList();
}
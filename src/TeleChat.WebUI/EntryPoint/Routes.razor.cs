using System.Reflection;
using TeleChat.WebUI.Layout;

namespace TeleChat.WebUI.EntryPoint;

public partial class Routes
{
    private readonly Type _defualtLayout = typeof(MainLayout);
    private readonly Assembly _appAssembly = typeof(App).Assembly;
    private readonly List<Assembly> _additionalAssemblies =
    [
        typeof(MainLayout).Assembly
    ];
}
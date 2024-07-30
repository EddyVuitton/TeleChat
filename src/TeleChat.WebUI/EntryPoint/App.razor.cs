using Microsoft.AspNetCore.Components.Web;

namespace TeleChat.WebUI.EntryPoint;

public partial class App
{
    private readonly InteractiveServerRenderMode _renderMode = new(prerender: false);
}
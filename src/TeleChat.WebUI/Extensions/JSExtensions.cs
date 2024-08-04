using Microsoft.JSInterop;

namespace TeleChat.Domain.Extensions;

public static class JSExtensions
{
    public static async ValueTask<object> SetInLocalStorage(this IJSRuntime js, string key, string content)
        => await js.InvokeAsync<object>("localStorage.setItem", key, content);

    public static ValueTask<string> GetFromLocalStorage(this IJSRuntime js, string key)
        => js.InvokeAsync<string>("localStorage.getItem", key);

    public static ValueTask<object> RemoveItemFromLocalStorage(this IJSRuntime js, string key)
        => js.InvokeAsync<object>("localStorage.removeItem", key);

    public static async Task LogAsync(this IJSRuntime js, string message)
        => await js.InvokeVoidAsync("console.log", message);
}
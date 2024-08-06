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

    public static async Task LogAsync(this IJSRuntime js, Exception ex)
       => await LogAsync(js, ExceptionToString(ex));

    private static string ExceptionToString(Exception ex)
    {
        var log = $"{ex.Message}";

        if (!string.IsNullOrEmpty(ex.InnerException?.ToString()))
        {
            log += Environment.NewLine + ex.InnerException;
        }

        if (!string.IsNullOrEmpty(ex.StackTrace?.ToString()))
        {
            log += Environment.NewLine + ex.StackTrace;
        }

        return log;
    }
}
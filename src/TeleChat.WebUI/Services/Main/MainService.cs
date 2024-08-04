using Newtonsoft.Json;
using System.Reflection;
using TeleChat.Domain.Auth;
using TeleChat.Domain.Forms;

namespace TeleChat.WebUI.Services.Main;

public class MainService(HttpClient httpClient) : IMainService
{
    private readonly HttpClient httpClient = httpClient;
    private const string _MainRoute = "api/Main";

    #region Publics

    public async Task<UserToken> LoginAsync(LoginAccountForm form)
    {
        var (issuer, audience) = GetIssuerAndAudience();

        var parameters = new List<string>()
        {
            $"login={form.Login}",
            $"password={form.Password}",
            $"issuer={issuer}",
            $"audience={audience}"
        };

        string queryString = string.Join("&", parameters);
        string url = $"{_MainRoute}/LoginAsync?{queryString}";

        var response = await httpClient.PostAsync(url, null);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<UserToken>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task AddToGroupAsync(string connectionId, string groupName)
    {
        var parameters = new List<string>
        {
            $"connectionId={connectionId}",
            $"groupName={groupName}"
        };

        string queryString = string.Join("&", parameters);
        string url = $"{_MainRoute}/AddToGroupAsync?{queryString}";

        var response = await httpClient.PostAsync(url, null);
        response.EnsureSuccessStatusCode();
    }

    public async Task SendToGroupAsync(string connectionId, string userName, string message, string groupName)
    {
        var parameters = new List<string>
        {
            $"connectionId={connectionId}",
            $"userName={userName}",
            $"message={message}",
            $"groupName={groupName}"
        };

        string queryString = string.Join("&", parameters);
        string url = $"{_MainRoute}/SendToGroupAsync?{queryString}";

        var response = await httpClient.PostAsync(url, null);
        response.EnsureSuccessStatusCode();
    }

    #endregion

    #region Privates

    private (string? issuer, string? audience) GetIssuerAndAudience()
    {
        var issuer = Assembly.GetAssembly(typeof(MainService))?.GetName().Name;
        var audience = httpClient.BaseAddress?.ToString();

        return (issuer, audience);
    }

    #endregion
}
using Newtonsoft.Json;
using TeleChat.Domain.Auth;

namespace TeleChat.WebUI.Services.Main;

public class MainService(HttpClient httpClient) : IMainService
{
    private readonly HttpClient httpClient = httpClient;
    private const string _MainRoute = "api/Main";

    public async Task<UserToken> BuildTokenAsync(string userName)
    {
        string url = $"{_MainRoute}/BuildToken?userName={userName}";

        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<UserToken>(responseContent);

        return deserialisedResponse ?? new();
    }
}
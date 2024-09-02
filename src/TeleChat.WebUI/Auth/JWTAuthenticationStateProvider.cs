using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using TeleChat.Domain.Auth;
using TeleChat.WebUI.Extensions;

namespace TeleChat.WebUI.Auth;

public class JWTAuthenticationStateProvider(IJSRuntime js, HttpClient httpClient) : AuthenticationStateProvider, ILoginService
{
    private readonly IJSRuntime _js = js;
    private readonly HttpClient _httpClient = httpClient;

    private const string _TOKENKEY = "TOKENKEY";
    private static AuthenticationState _anonymous => new(new ClaimsPrincipal(new ClaimsIdentity()));

    #region PublicMethods

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _js.GetFromLocalStorage(_TOKENKEY);

            if (string.IsNullOrEmpty(token))
                return _anonymous;

            return BuildAuthenticationState(token);
        }
        catch
        {
            return await Task.FromResult(_anonymous);
        }
    }

    public AuthenticationState BuildAuthenticationState(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
    }

    public async Task<(bool isLoggedIn, string? token, List<Claim>? userClaims)> IsLoggedInAsync()
    {
        var authenticationState = await GetAuthenticationStateAsync();

        if (authenticationState is not null && authenticationState.User.Claims.Any())
        {
            var token = await _js.GetFromLocalStorage(_TOKENKEY);
            var userClaims = authenticationState.User.Claims.ToList();

            return (true, token, userClaims);
        }

        return (false, null, null);
    }

    public async Task LogoutIfExpiredTokenAsync()
    {
        var now = DateTime.Now;
        var validTo = await TokenValidToAsync();

        if (validTo.CompareTo(now) <= 0)
        {
            await LogoutAsync();
        }
    }

    public async Task LoginAsync(UserToken userToken)
    {
        await _js.SetInLocalStorage(_TOKENKEY, userToken.Token);
        var authState = BuildAuthenticationState(userToken.Token);
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public async Task LogoutAsync()
    {
        await CleanUpAsync();
    }

    #endregion Publics

    #region PrivateMethods

    private static List<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        keyValuePairs!.TryGetValue(ClaimTypes.Role, out var roles);

        if (roles is not null)
        {
            if (roles.ToString()!.Trim().StartsWith('['))
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);

                if (parsedRoles is null)
                    return [];

                foreach (var parsedRole in parsedRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
            }

            keyValuePairs.Remove(ClaimTypes.Role);
        }

        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));
        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    private async Task CleanUpAsync()
    {
        await _js.RemoveItemFromLocalStorage(_TOKENKEY);
        _httpClient.DefaultRequestHeaders.Authorization = null;
        NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
    }

    private async Task<DateTime> TokenValidToAsync()
    {
        var token = await _js.GetFromLocalStorage(_TOKENKEY);

        if (!string.IsNullOrEmpty(token))
        {
            var validTo = new JwtSecurityTokenHandler().ReadToken(token).ValidTo.ToLocalTime();

            return validTo;
        }

        return DateTime.Now;
    }

    #endregion Privates
}
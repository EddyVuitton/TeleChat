﻿using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using TeleChat.Domain.Auth;
using TeleChat.Domain.Models.Entities;
using TeleChat.Domain.Forms;
using static MudBlazor.CategoryTypes;

namespace TeleChat.WebUI.Account;

public class AccountService(HttpClient httpClient) : IAccountService
{
    private readonly HttpClient _httpClient = httpClient;
    private const string _AccountRoute = "api/Account";

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
        string url = $"{_AccountRoute}/LoginAsync?{queryString}";

        var response = await _httpClient.PostAsync(url, null);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<UserToken>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task RegisterAsync(RegisterAccountForm form)
    {
        var json = JsonConvert.SerializeObject(form);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_AccountRoute}/RegisterAsync", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<User?> GetUserByLoginAsync(string login)
    {
        string url = $"{_AccountRoute}/GetUserByLoginAsync?login={login}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<User>(responseContent);

        return deserialisedResponse;
    }

    public async Task<UserToken> GetTokenAsync()
    {
        var (issuer, audience) = GetIssuerAndAudience();

        var parameters = new List<string>()
        {
            $"issuer={issuer}",
            $"audience={audience}"
        };

        string queryString = string.Join("&", parameters);
        string url = $"{_AccountRoute}/GetToken?{queryString}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<UserToken>(responseContent);

        return deserialisedResponse ?? new();
    }

    public async Task<User?> CreateUser(string name)
    {
        string url = $"{_AccountRoute}/CreateUser?name={name}";

        var response = await _httpClient.PostAsync(url, null);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deserialisedResponse = JsonConvert.DeserializeObject<User>(responseContent);

        return deserialisedResponse;
    }

    #endregion

    #region Privates

    private (string? issuer, string? audience) GetIssuerAndAudience()
    {
        var issuer = Assembly.GetAssembly(typeof(AccountService))?.GetName().Name;
        var audience = _httpClient.BaseAddress?.ToString();

        return (issuer, audience);
    }

    #endregion
}
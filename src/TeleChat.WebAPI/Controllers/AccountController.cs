﻿using Microsoft.AspNetCore.Mvc;
using TeleChat.Domain.Auth;
using TeleChat.Domain.Models.Entities;
using TeleChat.Domain.Forms;
using TeleChat.WebAPI.Repositories.Account;

namespace TeleChat.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountRepository accountRepository, ILogger<AccountController> logger) : ControllerBase
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly ILogger<AccountController> _logger = logger;

    [HttpPost("Login")]
    public async Task<ActionResult<UserToken>> Login(string login, string password, string issuer, string audience)
    {
        try
        {
            var token = await _accountRepository.LoginAsync(login, password, issuer, audience);
            return Ok(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd w logowaniu");
            return Problem(ex.Message);
        }
    }

    [HttpPost("Register")]
    public async Task<ActionResult> Register(RegisterAccountForm form)
    {
        try
        {
            await _accountRepository.RegisterAsync(form);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd w rejestracji");
            return Problem(ex.Message);
        }
    }

    [HttpGet("GetUserByLogin")]
    public async Task<ActionResult<User>> GetUserByLogin(string login)
    {
        try
        {
            var result = await _accountRepository.GetUserByLoginAsync(login);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd w znalezieniu użytkownika");
            return Problem(ex.Message);
        }
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult<User>> CreateUser(string name)
    {
        try
        {
            var result = await _accountRepository.CreateUser(name);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd w znalezieniu użytkownika");
            return Problem(ex.Message);
        }
    }
}
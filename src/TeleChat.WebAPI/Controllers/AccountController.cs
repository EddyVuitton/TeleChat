using Microsoft.AspNetCore.Mvc;
using TeleChat.Domain.Auth;
using TeleChat.Domain.Entities;
using TeleChat.Domain.Forms;
using TeleChat.WebAPI.Repositories;

namespace TeleChat.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(AccountRepository accountRepository, ILogger<MainController> logger) : ControllerBase
{
    private readonly AccountRepository _accountRepository = accountRepository;
    private readonly ILogger<MainController> _logger = logger;

    [HttpPost("LoginAsync")]
    public async Task<ActionResult<UserToken>> LoginAsync(string login, string password, string issuer, string audience)
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

    [HttpPost("RegisterAsync")]
    public async Task<ActionResult> RegisterAsync(RegisterAccountForm form)
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

    [HttpGet("GetUserByLoginAsync")]
    public async Task<ActionResult<User>> GetUserByLoginAsync(string login)
    {
        try
        {
            await _accountRepository.GetUserByLoginAsync(login);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd w znalezieniu użytkownika");
            return Problem(ex.Message);
        }
    }
}
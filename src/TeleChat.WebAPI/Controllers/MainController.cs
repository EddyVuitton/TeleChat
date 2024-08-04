using Microsoft.AspNetCore.Mvc;
using TeleChat.Domain.Auth;
using TeleChat.WebAPI.Repositories;

namespace TeleChat.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MainController(MainRepository mainRepository, AccountRepository accountRepository, ILogger<MainController> logger) : ControllerBase
{
    private readonly MainRepository _mainRepository = mainRepository;
    private readonly AccountRepository _accountRepository = accountRepository;
    private readonly ILogger<MainController> _logger = logger;

    [HttpPost("AddToGroupAsync")]
    public async Task<ActionResult> AddToGroupAsync(string connectionId, string groupName)
    {
        try
        {
            await _mainRepository.AddToGroupAsync(connectionId, groupName);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd w dodaniu połączenia do grupy");
            return Problem(ex.Message);
        }
    }

    [HttpPost("SendToGroupAsync")]
    public async Task<ActionResult> SendToGroupAsync(string connectionId, string userName, string message, string groupName)
    {
        try
        {
            await _mainRepository.SendToGroupAsync(connectionId, userName, message, groupName);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd w wysłaniu wiadomości do grupy");
            return Problem(ex.Message);
        }
    }

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
}
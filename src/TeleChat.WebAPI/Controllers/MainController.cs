using Microsoft.AspNetCore.Mvc;
using TeleChat.WebAPI.Repositories;

namespace TeleChat.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MainController(MainRepository mainRepository, ILogger<MainController> logger) : ControllerBase
{
    private readonly MainRepository _mainRepository = mainRepository;
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
}
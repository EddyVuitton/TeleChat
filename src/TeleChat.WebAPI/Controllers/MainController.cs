using Microsoft.AspNetCore.Mvc;
using TeleChat.Domain.Dtos;
using TeleChat.Domain.Entities;
using TeleChat.WebAPI.Repositories.Main;

namespace TeleChat.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MainController(IMainRepository mainRepository, ILogger<MainController> logger) : ControllerBase
{
    private readonly IMainRepository _mainRepository = mainRepository;
    private readonly ILogger<MainController> _logger = logger;

    [HttpPost("AddConnectionToGroupAsync")]
    public async Task<ActionResult> AddConnectionToGroupAsync(string connectionId, Guid groupChatGuid)
    {
        try
        {
            await _mainRepository.AddConnectionToGroupAsync(connectionId, groupChatGuid);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd w dodaniu połączenia do grupy");
            return Problem(ex.Message);
        }
    }

    [HttpPost("SendMessageAsync")]
    public async Task<ActionResult<Message>> SendMessageAsync(MessageDto message)
    {
        try
        {
            var result = await _mainRepository.SendMessageAsync(message);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd w wysłaniu wiadomości");
            return Problem(ex.Message);
        }
    }

    [HttpGet("GetMessageTypesAsync")]
    public async Task<ActionResult<List<MessageType>>> GetMessageTypesAsync()
    {
        try
        {
            var result = await _mainRepository.GetMessageTypesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd w pobraniu typów wiadomości");
            return Problem(ex.Message);
        }
    }

    [HttpGet("GetUserGroupChatsAsync")]
    public async Task<ActionResult<List<UserGroupChat>>> GetUserGroupChatsAsync(int userId)
    {
        try
        {
            var result = await _mainRepository.GetUserGroupChatsAsync(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd w pobraniu grup użytkownika");
            return Problem(ex.Message);
        }
    }
}
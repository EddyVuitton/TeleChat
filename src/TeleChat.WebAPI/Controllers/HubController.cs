using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeleChat.Domain;
using TeleChat.Domain.Models.Entities;
using TeleChat.WebAPI.Repositories.Hub;

namespace TeleChat.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HubController(IHubRepository repository, ILogger<HubController> logger, DBContext context) : ControllerBase
{
    private readonly IHubRepository _repository = repository;
    private readonly ILogger<HubController> _logger = logger;
    private readonly DBContext _context = context;

    [HttpPost("AddConnectionToGroupAsync")]
    public async Task<ActionResult> AddConnectionToGroupAsync(string connectionId, Guid groupChatGuid)
    {
        try
        {
            await _repository.AddConnectionToGroupAsync(connectionId, groupChatGuid);
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
            var result = await _repository.SendMessageAsync(message);
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
            var result = await _repository.GetMessageTypesAsync();
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
            var result = await _repository.GetUserGroupChatsAsync(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd w pobraniu grup użytkownika");
            return Problem(ex.Message);
        }
    }

    [HttpGet("GetDefaultGroupChat")]
    public async Task<ActionResult<GroupChat>> GetDefaultGroupChatAsync()
    {
        try
        {
            var result = await _repository.GetDefaultGroupChatAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            //to do...
            return Problem(ex.Message);
        }
    }

    [HttpGet("GetGroupChatMessages")]
    public async Task<ActionResult<List<Message>>> GetGroupChatMessagesAsync(int groupChatId)
    {
        try
        {
            var result = await _repository.GetGroupChatMessagesAsync(groupChatId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            //to do...
            return Problem(ex.Message);
        }
    }
}
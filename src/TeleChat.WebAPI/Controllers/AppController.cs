using Microsoft.AspNetCore.Mvc;
using TeleChat.Domain;
using TeleChat.Domain.Models.Entities;
using TeleChat.WebAPI.Repositories.App;

namespace TeleChat.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppController(IAppRepository repository, ILogger<AppController> logger) : ControllerBase
{
    private readonly IAppRepository _repository = repository;
    private readonly ILogger<AppController> _logger = logger;

    [HttpPost("AddConnectionToGroup")]
    public async Task<ActionResult> AddConnectionToGroup(string connectionId, Guid groupChatGuid)
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

    [HttpPost("SendMessage")]
    public async Task<ActionResult<Message>> SendMessage(MessageDto message)
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

    [HttpPost("AddGroupChat")]
    public async Task<ActionResult<UserGroupChat>> AddGroupChat(GroupChatDto groupChat)
    {
        try
        {
            var result = await _repository.AddGroupChatAsync(groupChat);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd w dodaniu grupy");
            return Problem(ex.Message);
        }
    }

    [HttpPost("DeleteGroupChat")]
    public async Task<ActionResult> DeleteGroupChat(int groupChatId)
    {
        try
        {
            await _repository.DeleteGroupChatAsync(groupChatId);
            return Ok();
        }
        catch (Exception ex)
        {
            //to do...
            return Problem(ex.Message);
        }
    }

    [HttpPost("DeleteMessage")]
    public async Task<ActionResult> DeleteMessage(int messageId)
    {
        try
        {
            await _repository.DeleteMessageAsync(messageId);
            return Ok();
        }
        catch (Exception ex)
        {
            //to do...
            return Problem(ex.Message);
        }
    }

    [HttpGet("GetMessageTypes")]
    public async Task<ActionResult<List<MessageType>>> GetMessageTypes()
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

    [HttpGet("GetUserGroupChats")]
    public async Task<ActionResult<List<UserGroupChat>>> GetUserGroupChats(int userId)
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
    public async Task<ActionResult<GroupChat>> GetDefaultGroupChat()
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
    public async Task<ActionResult<List<Message>>> GetGroupChatMessages(int groupChatId)
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
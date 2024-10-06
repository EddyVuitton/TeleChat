using Microsoft.AspNetCore.Mvc;
using TeleChat.WebAPI.Files;

namespace TeleChat.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController(IFileService fileService, ILogger<AccountController> logger) : ControllerBase
{
    private readonly IFileService _fileService = fileService;
    private readonly ILogger<AccountController> _logger = logger;

    [HttpPost("SaveFile")]
    public async Task<ActionResult<string>> SaveFile(IFormFile? file)
    {
        try
        {
            var result = await _fileService.SaveFileAsync(file);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd w zapisaniu pliku");
            return Problem(ex.Message);
        }
    }
}
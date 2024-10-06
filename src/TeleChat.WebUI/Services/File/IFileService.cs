using Microsoft.AspNetCore.Components.Forms;

namespace TeleChat.WebUI.Services.File;

public interface IFileService
{
    Task<string?> SaveFileAsync(IBrowserFile file);
}
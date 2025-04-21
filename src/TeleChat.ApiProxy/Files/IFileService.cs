using Microsoft.AspNetCore.Components.Forms;

namespace TeleChat.ApiProxy.Files;

public interface IFileService
{
    Task<string?> SaveFileAsync(IBrowserFile file);
}
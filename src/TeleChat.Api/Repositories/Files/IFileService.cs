namespace TeleChat.Api.Repositories.Files;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile? file);
}
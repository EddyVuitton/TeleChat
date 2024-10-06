namespace TeleChat.WebAPI.Files;

public interface IFileService
{
    Task<byte[]> GetFileAsync(string fileName);
    Task<string> SaveFileAsync(IFormFile? file);
}
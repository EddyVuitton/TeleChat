namespace TeleChat.Api.Repositories.Files;

public class FileService : IFileService
{
    private readonly string _uploadPath;

    public FileService(IWebHostEnvironment env)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        
        _uploadPath = env.IsProduction() ? 
            Path.Combine(currentDirectory, "wwwroot", "images") : 
            Path.Combine(Directory.GetParent(currentDirectory)!.FullName, "TeleChat.Server", "wwwroot", "images");

        Directory.CreateDirectory(_uploadPath);
    }

    public async Task<string> SaveFileAsync(IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            throw new Exception("Niepoprawny plik");
        }

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(_uploadPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/images/{fileName}";
    }
}
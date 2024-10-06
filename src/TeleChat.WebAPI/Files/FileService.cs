using Microsoft.Extensions.Options;
using TeleChat.WebAPI.Options.FilesContainer;

namespace TeleChat.WebAPI.Files
{
    public class FileService(IWebHostEnvironment env, IOptions<FilesContainerOptions> options) : IFileService
    {
        private readonly string _uploadPath = env.ContentRootPath.Replace(env.ApplicationName, options.Value.Path);

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

            return $"./images/{fileName}";
        }

        public async Task<byte[]> GetFileAsync(string fileName)
        {
            var filePath = Path.Combine(_uploadPath, fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Nie znaleziono pliku");
            }

            var file = await File.ReadAllBytesAsync(filePath);

            return file;
        }
    }
}
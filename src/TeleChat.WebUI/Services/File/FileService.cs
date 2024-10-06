using Microsoft.JSInterop;
using TeleChat.WebUI.Extensions;
using Microsoft.AspNetCore.Components.Forms;

namespace TeleChat.WebUI.Services.File;

public class FileService(HttpClient httpClient, IJSRuntime js) : IFileService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IJSRuntime _js = js;
    private const string _FileRoute = "api/File";

    public async Task<string?> SaveFileAsync(IBrowserFile file)
    {
        try
        {
            var content = new MultipartFormDataContent();
            var fileStream = file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 15); // maksymalny rozmiar 15MB
            
            var fileContent = new StreamContent(fileStream);
            content.Add(fileContent, "file", file.Name);

            var response = await _httpClient.PostAsync($"{_FileRoute}/SaveFile", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
        catch (Exception ex)
        {
            await _js.LogAsync(ex);
            return null;
        }
    }
}
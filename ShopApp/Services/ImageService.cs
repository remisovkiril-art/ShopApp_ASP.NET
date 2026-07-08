using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ShopApi.Interfaces;

namespace ShopApi.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly string _dirname;

    public ImageService(IWebHostEnvironment environment, IConfiguration configuration)
    {
        _environment = environment;
        _dirname = configuration["DirnameForFiles:Categories"] ?? "categories";
    }
    public async Task<string> SaveFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is empty.");
        }
        var folderPath = Path.Combine(_environment.WebRootPath, _dirname);
        Directory.CreateDirectory(folderPath);
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(folderPath, fileName);
        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return fileName;
    }
}

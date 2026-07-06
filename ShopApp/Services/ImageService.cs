using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ShopApi.Interfaces;

namespace ShopApi.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly string _dirname = "images";

    public ImageService(IWebHostEnvironment environment)
    {
        _environment = environment;
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

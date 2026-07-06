namespace ShopApi.Interfaces;

public interface IImageService
{
    Task<string> SaveFileAsync(IFormFile file);
}
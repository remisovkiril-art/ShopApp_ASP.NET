using ShopApplication.DTOs.ProductDTOs;

namespace ShopApplication.Interfaces.Services;

public interface IProductService
{
    Task<int> CreateProductAsync(ProductCreateDTO dto);
    Task<List<ProductReadDTO>> GetAllProductsAsync();
    Task<ProductReadDTO?> GetProductByIdAsync(int id);
}

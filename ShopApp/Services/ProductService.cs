using AutoMapper;
using ShopApplication.DTOs.ProductDTOs;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using ShopDomain.Models;

namespace ShopApplication.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> CreateProductAsync(ProductCreateDTO dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            StockQty = dto.StockQty,
            CategoryId = dto.CategoryId,
            IsActive = true,
            Images = dto.ImageUrls.Select((url, index) => new ProductImage
            {
                Url = url,
                IsPrimary = index == 0
            }).ToList()
        };

        return await _repository.CreateProductAsync(product);
    }

    public async Task<List<ProductReadDTO>> GetAllProductsAsync()
    {
        var products = await _repository.GetAllProductsAsync();
        return _mapper.Map<List<ProductReadDTO>>(products);
    }

    public async Task<ProductReadDTO?> GetProductByIdAsync(int id)
    {
        var product = await _repository.GetProductByIdAsync(id);
        return product == null ? null : _mapper.Map<ProductReadDTO>(product);
    }
}
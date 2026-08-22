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
    private readonly ICachingService _cacheService;

    public ProductService(
        IProductRepository repository,
        IMapper mapper,
        ICachingService cacheService)
    {
        _repository = repository;
        _mapper = mapper;
        _cacheService = cacheService;
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

        var result = await _repository.CreateProductAsync(product);

        await _cacheService.RemoveAsync("Products");

        return result;
    }

    public async Task<List<ProductReadDTO>> GetAllProductsAsync()
    {
        var cache = await _cacheService.GetAsync<List<ProductReadDTO>>("Products");

        if (cache != null)
        {
            return cache;
        }

        var products = await _repository.GetAllProductsAsync();

        var dtos = _mapper.Map<List<ProductReadDTO>>(products);

        await _cacheService.SetAsync("Products", dtos, null);

        return dtos;
    }

    public async Task<ProductReadDTO?> GetProductByIdAsync(int id)
    {
        var product = await _repository.GetProductByIdAsync(id);

        return product == null
            ? null
            : _mapper.Map<ProductReadDTO>(product);
    }
}
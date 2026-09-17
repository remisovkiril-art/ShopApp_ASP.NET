using MediatR;
using ShopApplication.DTOs.ProductDTOs;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using ProductEntity = ShopDomain.Models.Product;
using ProductImageEntity = ShopDomain.Models.ProductImage;

namespace ShopApplication.Commands.Product;

public class CreateProductCommand : IRequest<int>
{
    public ProductCreateDTO DTO { get; }

    public CreateProductCommand(ProductCreateDTO dto)
    {
        DTO = dto;
    }
}

public class CreateProductCommandHandler
    : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductRepository _repository;
    private readonly ICachingService _cacheService;

    public CreateProductCommandHandler(
        IProductRepository repository,
        ICachingService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<int> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        ProductEntity product = new ProductEntity
        {
            Name = request.DTO.Name,
            Description = request.DTO.Description,
            Price = request.DTO.Price,
            StockQty = request.DTO.StockQty,
            CategoryId = request.DTO.CategoryId,
            IsActive = true,
            Images = request.DTO.ImageUrls
                .Select((url, index) => new ProductImageEntity
                {
                    Url = url,
                    IsPrimary = index == 0
                })
                .ToList()
        };

        int result = await _repository.CreateProductAsync(
            product,
            cancellationToken);

        await _cacheService.RemoveAsync("Products");

        return result;
    }
}
using MediatR;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;

namespace ShopApplication.Commands.Product;

public class DeleteProductHandler
    : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _repository;
    private readonly ICachingService _cacheService;

    public DeleteProductHandler(
        IProductRepository repository,
        ICachingService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        await _repository.DeleteProductAsync(
            request.Id,
            cancellationToken);

        await _cacheService.RemoveAsync("Products");

        await _cacheService.RemoveAsync(
            $"Product:{request.Id}");
    }
}

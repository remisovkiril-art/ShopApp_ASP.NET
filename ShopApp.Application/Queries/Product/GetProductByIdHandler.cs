using AutoMapper;
using MediatR;
using ShopApplication.DTOs.ProductDTOs;
using ShopApplication.Interfaces.Repository;

namespace ShopApplication.Queries.Product;

public class GetProductByIdHandler
    : IRequestHandler<GetProductByIdQuery, ProductReadDTO?>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public GetProductByIdHandler(
        IProductRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProductReadDTO?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetProductByIdAsync(
            request.Id,
            cancellationToken);

        return _mapper.Map<ProductReadDTO?>(entity);
    }
}
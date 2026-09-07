using MediatR;

namespace ShopApplication.Commands.Product;

public class DeleteProductCommand(int id) : IRequest
{
    public int Id { get; } = id;
}

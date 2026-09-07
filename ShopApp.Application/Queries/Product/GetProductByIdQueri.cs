using MediatR;
using ShopApplication.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApplication.Queries.Product;

public record GetProductByIdQuery(int id) : IRequest<ProductReadDTO?>
{
    public int Id { get; } = id;
}

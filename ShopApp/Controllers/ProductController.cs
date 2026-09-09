using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Requests.Products;
using ShopApplication.Commands.Product;
using ShopApplication.DTOs.ProductDTOs;
using ShopApplication.Queries.Product;
using IImageService = ShopApi.Interfaces.IImageService;
using IProductService = ShopApplication.Interfaces.Services.IProductService;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IImageService _imageService;
    private readonly IMediator _mediator;
    private readonly int _maxImages;

    public ProductController(
        IProductService productService,
        IImageService imageService,
        IConfiguration configuration,
        IMediator mediator)
    {
        _productService = productService;
        _imageService = imageService;
        _mediator = mediator;
        _maxImages = configuration.GetValue<int?>(
            "ProductSettings:MaxImages") ?? 5;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        [FromForm] ProductCreateRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Images.Count > _maxImages)
        {
            return BadRequest(
                $"Максимальное количество изображений для продукта: {_maxImages}.");
        }

        var imageUrls = new List<string>();

        foreach (var image in request.Images)
        {
            imageUrls.Add(
                await _imageService.SaveFileAsync(
                    image,
                    cancellationToken));
        }

        var id = await _productService.CreateProductAsync(
            new ProductCreateDTO
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQty = request.StockQty,
                CategoryId = request.CategoryId,
                ImageUrls = imageUrls
            },
            cancellationToken);

        return CreatedAtAction(
            nameof(GetProductById),
            new { id },
            new { id });
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductReadDTO>>> GetProducts(
        CancellationToken cancellationToken)
    {
        return Ok(
            await _productService.GetAllProductsAsync(
                cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductReadDTO>> GetProductById(
        int id,
        CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(
            new GetProductByIdQuery(id),
            cancellationToken);

        return product == null
            ? NotFound("Продукт не найден")
            : Ok(product);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(
        int id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteProductCommand(id),
            cancellationToken);

        return NoContent();
    }
}
using Microsoft.AspNetCore.Mvc;
using ShopApi.Requests.Products;
using ShopApplication.DTOs.ProductDTOs;
using IImageService = ShopApi.Interfaces.IImageService;
using IProductService = ShopApplication.Interfaces.Services.IProductService;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IImageService _imageService;
    private readonly int _maxImages;

    public ProductController(
        IProductService productService,
        IImageService imageService,
        IConfiguration configuration)
    {
        _productService = productService;
        _imageService = imageService;
        _maxImages = configuration.GetValue<int?>("ProductSettings:MaxImages") ?? 5;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromForm] ProductCreateRequest request)
    {
        if (request.Images.Count > _maxImages)
        {
            return BadRequest($"Максимальное количество изображений для продукта: {_maxImages}.");
        }

        var imageUrls = new List<string>();
        foreach (var image in request.Images)
        {
            imageUrls.Add(await _imageService.SaveFileAsync(image));
        }

        var id = await _productService.CreateProductAsync(new ProductCreateDTO
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQty = request.StockQty,
            CategoryId = request.CategoryId,
            ImageUrls = imageUrls
        });

        return CreatedAtAction(nameof(GetProductById), new { id }, new { id });
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductReadDTO>>> GetProducts()
    {
        return Ok(await _productService.GetAllProductsAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductReadDTO>> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return product == null ? NotFound("Продукт не найден") : Ok(product);
    }
}
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ShopApi.Requests.Products;

public class ProductCreateRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int StockQty { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public List<IFormFile> Images { get; set; } = new();
}

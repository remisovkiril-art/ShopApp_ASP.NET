using System.ComponentModel.DataAnnotations;

namespace ShopApplication.DTOs.ProductDTOs;

public class ProductCreateDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
    [Range(0, int.MaxValue)]
    public int StockQty { get; set; }
    public int CategoryId { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}

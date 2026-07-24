namespace ShopApplication.DTOs.ProductDTOs;

public class ProductReadDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQty { get; set; }
    public int CategoryId { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}

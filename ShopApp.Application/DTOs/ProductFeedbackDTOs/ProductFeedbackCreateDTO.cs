namespace ShopApplication.DTOs.ProductFeedbackDTOs;

public class ProductFeedbackCreateDTO
{
    public int ProductId { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
}
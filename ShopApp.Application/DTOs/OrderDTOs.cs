namespace ShopApplication.DTOs.OrderDTOs;

public class OrderDetailCreateDTO
{
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
}

public class OrderCreateDTO
{
    public Guid UserId { get; set; }
    public string Status { get; set; } = null!;
    public bool Paid { get; set; }
    public ICollection<OrderDetailCreateDTO> OrderDetails { get; set; } = new List<OrderDetailCreateDTO>();
}

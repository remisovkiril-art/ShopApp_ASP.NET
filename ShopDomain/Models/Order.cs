namespace ShopDomain.Models;

public class Order
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string Status { get; set; } = null!;

    public bool Paid { get; set; }

    public User User { get; set; } = null!;

    public ICollection<OrderDetail> OrderDetails { get; set; }
        = new List<OrderDetail>();
}
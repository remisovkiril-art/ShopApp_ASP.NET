using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopDomain.Models;

[Table("delivery_addresses")]
public class DeliveryAddress : BaseEntity
{
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Required]
    [Column("city")]
    public string City { get; set; } = string.Empty;

    [Required]
    [Column("street")]
    public string Street { get; set; } = string.Empty;

    [Required]
    [Column("house")]
    public string House { get; set; } = string.Empty;

    [Column("apartment")]
    public string? Apartment { get; set; }

    public User? User { get; set; }
}
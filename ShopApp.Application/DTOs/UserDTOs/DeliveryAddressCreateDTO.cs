using System.ComponentModel.DataAnnotations;

namespace ShopApplication.DTOs.UserDTOs;

public class DeliveryAddressCreateDTO
{
    [Required]
    public string City { get; set; } = string.Empty;

    [Required]
    public string Street { get; set; } = string.Empty;

    [Required]
    public string House { get; set; } = string.Empty;

    public string? Apartment { get; set; }
}
namespace ShopApplication.DTOs.UserDTOs;

public class DeliveryAddressReadDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string House { get; set; } = string.Empty;
    public string? Apartment { get; set; }
}
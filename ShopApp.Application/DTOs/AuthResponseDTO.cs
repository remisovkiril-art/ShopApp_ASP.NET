using ShopApplication.DTOs.UserDTOs;

namespace ShopApplication.DTOs;

public class AuthResponseDTO
{
    public UserReadDTO? User { get; set; }

    public string? Token { get; set; }

    public string? RefreshToken { get; set; }
}
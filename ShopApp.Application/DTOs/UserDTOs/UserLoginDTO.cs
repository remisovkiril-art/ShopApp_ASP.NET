using System.ComponentModel.DataAnnotations;

namespace ShopApplication.DTOs.UserDTOs;

public class UserLoginDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}

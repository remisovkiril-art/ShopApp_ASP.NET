using System.ComponentModel.DataAnnotations;

namespace ShopApplication.DTOs.UserDTOs;

public class AdminCreateDTO
{
    [Required]
    [MinLength(5)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(5)]
    public string Password { get; set; } = null!;
}
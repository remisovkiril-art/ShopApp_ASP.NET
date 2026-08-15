using System.ComponentModel.DataAnnotations;
using ShopDomain.Enums;

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

    [Required]
    public UserRole Role { get; set; }
}
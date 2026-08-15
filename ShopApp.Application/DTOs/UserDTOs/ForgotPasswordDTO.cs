using System.ComponentModel.DataAnnotations;

namespace ShopApplication.DTOs.UserDTOs;

public class ForgotPasswordDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}
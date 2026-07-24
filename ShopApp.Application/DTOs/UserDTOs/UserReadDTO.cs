using ShopDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApplication.DTOs.UserDTOs;
public class UserReadDTO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.User;

}
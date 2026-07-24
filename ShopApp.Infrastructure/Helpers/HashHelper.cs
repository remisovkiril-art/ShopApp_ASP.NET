using ShopApplication.Interfaces.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopInfrastructure.Helpers;
public class HashHelper : IHashHelper
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool IsValidPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}
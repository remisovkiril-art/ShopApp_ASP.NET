using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApplication.Interfaces.Helpers;
public interface IHashHelper
{
    public string Hash(string password);
    public bool IsValidPassword(string password, string hash);
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopDomain.Models;
namespace ShopApplication.Interfaces.Repository;

public interface ICategoryRepository
{
    Task<int> AddCategoryAsync(Category category);
    Task<List<Category>> GetAllCategoriesAsync();
}

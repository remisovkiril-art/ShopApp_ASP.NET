using ShopApi.homework3.Models;

namespace ShopApi.homework3.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        Product? GetById(int id);
        Product Create(ProductDto dto);
        Product? Update(int id, ProductDto dto);
        bool Delete(int id);
        IEnumerable<Product> SearchByName(string name);
    }
}


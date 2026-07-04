using ShopDomain.Models;
namespace ShopApi.Interfaces;

public interface IProductService
{
    List<Product> GetAllProducts();
    void AddProduct(Product product);
}

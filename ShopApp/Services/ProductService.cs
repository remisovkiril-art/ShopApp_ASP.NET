using ShopApi.Interfaces;
using ShopDomain.Models;

namespace ShopApi.Services
{
    public class ProductService : IProductService
    {
        private List<Product> _products = new();
        public ProductService()
        {
            _products.Add(new Product()
            {
                Name = "Milk",
                Price = (decimal)40.9f
            });

            _products.Add(new Product()
            {
                Name = "Bread",
                Price = (decimal)30.5f
            });
        }

        public List<Product> GetAllProducts()
        {
            return _products;
        }

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }
    }
}

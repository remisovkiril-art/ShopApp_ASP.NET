using ShopApi.homework3.Models;

namespace ShopApi.homework3.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Laptop", Price = 1500.00m },
            new Product { Id = 2, Name = "Smartphone", Price = 800.00m },
            new Product { Id = 3, Name = "Headphones", Price = 150.00m }
        };

        private int _nextId = 4;

        public IEnumerable<Product> GetAll() => _products;

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public Product Create(ProductDto dto)
        {
            var product = new Product
            {
                Id = _nextId++,
                Name = dto.Name,
                Price = dto.Price
            };
            _products.Add(product);
            return product;
        }

        public Product? Update(int id, ProductDto dto)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return null;

            product.Name = dto.Name;
            product.Price = dto.Price;
            return product;
        }

        public bool Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return false;

            _products.Remove(product);
            return true;
        }
        public IEnumerable<Product> SearchByName(string name)
        {
            return _products.Where(p => p.Name.ToLower().Contains(name.ToLower()));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Shop.App.Filters;
using ShopApp.Interfaces;
using ShopDomain.Models;

namespace Shop.App.Controllers;

// http://localhost:port/api/product
[ApiController]
[Route("api/[controller]")]
[LogActionFilter]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public List<Product> GetProducts()
    {
        return productService.GetAllProducts();
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById([FromRoute] int id)
    {
        var product = new Product()
        {
            Title = $"Test Product {id}",
            Price = 100
        };
        return Ok(product);
    }

    [HttpPost]
    public IActionResult AddNewProduct(Product product)
    {
        productService.AddProduct(product);
        return CreatedAtAction(nameof(GetProductById), new { id = 0 }, product);
    }
}




















//using Microsoft.AspNetCore.Mvc;
//namespace ShopApp.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ItemsController : ControllerBase
//    {
//        [HttpGet]
//        public IActionResult GetItems()
//        {
//            return Ok("Заглушка для всех товаров Статус 200 ОК");
//        }
//        [HttpGet("{id}")]
//        public IActionResult GetItemById(int id)
//        {
//            return Ok($"Заглушка для товара с ID: {id} Статус 200 OK");
//        }
//    }
//}

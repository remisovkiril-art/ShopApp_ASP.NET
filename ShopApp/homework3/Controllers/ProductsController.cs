//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using ShopApi.homework3.Models;
//using ShopApi.homework3.Services;

//namespace ShopApi.homework3.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ProductsController : ControllerBase
//    {
//        private readonly IProductService _productService;

//        public ProductsController(IProductService productService)
//        {
//            _productService = productService;
//        }

//        [HttpGet]
//        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
//        public ActionResult<IEnumerable<Product>> GetProducts()
//        {
//            return Ok(_productService.GetAll());
//        }

//        [HttpGet("{id:int}", Name = "GetProductById")]
//        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public ActionResult<Product> GetProductById(int id)
//        {
//            var product = _productService.GetById(id);
//            if (product == null)
//            {
//                return NotFound("Product not found");
//            }
//            return Ok(product);
//        }

//        [HttpGet("search")]
//        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public ActionResult<IEnumerable<Product>> SearchProducts([FromQuery] string? name)
//        {
//            if (string.IsNullOrWhiteSpace(name))
//            {
//                return BadRequest("Параметр 'name' является обязательным для поиска.");
//            }

//            var results = _productService.SearchByName(name);
//            return Ok(results);
//        }

//        [HttpPost]
//        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Product))]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public ActionResult<Product> CreateProduct([FromBody] ProductDto dto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var newProduct = _productService.Create(dto);
//            return CreatedAtRoute("GetProductById", new { id = newProduct.Id }, newProduct);
//        }

//        [HttpPut("{id:int}")]
//        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public ActionResult<Product> UpdateProduct(int id, [FromBody] ProductDto dto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var updatedProduct = _productService.Update(id, dto);
//            if (updatedProduct == null)
//            {
//                return NotFound("Product not found");
//            }

//            return Ok(updatedProduct);
//        }

//        [HttpDelete("{id:int}")]
//        [ProducesResponseType(StatusCodes.Status204NoContent)]
//        [ProducesResponseType(StatusCodes.Status404NotFound)]
//        public ActionResult DeleteProduct(int id)
//        {
//            var success = _productService.Delete(id);
//            if (!success)
//            {
//                return NotFound("Product not found");
//            }

//            return NoContent();
//        }
//    }
//}

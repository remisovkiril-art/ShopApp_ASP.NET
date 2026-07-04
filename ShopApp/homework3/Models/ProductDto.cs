using System.ComponentModel.DataAnnotations;

namespace ShopApi.homework3.Models
{
    public class ProductDto
    {
        [Required(ErrorMessage = "Поле Name является обязательным")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше нуля")]
        public decimal Price { get; set; }
    }
}

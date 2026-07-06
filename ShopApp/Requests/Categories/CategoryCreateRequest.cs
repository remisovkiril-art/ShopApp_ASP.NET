using Microsoft.AspNetCore.Http;

namespace ShopApi.Requests.Categories
{
    public class CategoryCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public IFormFile? Image { get; set; }
    }
}

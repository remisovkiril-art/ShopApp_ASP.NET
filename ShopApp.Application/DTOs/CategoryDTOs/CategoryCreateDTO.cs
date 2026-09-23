using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApplication.DTOs.CategoryDTOs;

[Table("categories")]
public class CategoryCreateDTO
{
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public int? ParentId { get; set; } = null;
}

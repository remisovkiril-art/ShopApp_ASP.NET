namespace ShopApplication.DTOs.CategoryDTOs;
public class CategoryUpdateDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public int? ParentId { get; set; }
}
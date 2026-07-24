namespace ShopApplication.DTOs.CategoryDTOs;

public class CategoryNodeDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public List<CategoryNodeDTO> Children { get; set; } = new();
}

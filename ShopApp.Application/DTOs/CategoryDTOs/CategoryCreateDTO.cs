using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApplication.DTOs.CategoryDTOs;

[Table("categories")]
public class CategoryCreateDTO
{
    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public int? ParentId { get; set; } = null;

    //public List<int> Products { get; set; } = new List<int>();
}
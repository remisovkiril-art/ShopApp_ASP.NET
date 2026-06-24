//using System;
//using Shop.App.practice1.Interfaces;
//using Shop.App.practice1.Models;
//namespace Shop.App.practice1.Services;
//public class CategoryService : ICategoryService
//{
//    private List<Category> _categories = new();
//    public CategoryService()
//    {
//        _categories.Add(new Category()
//        {
//            Id = 1,
//            Title = "Electronics",
//            Description = "Gadgets and devices",
//            Image = "electronics.jpg",
//            CreatedAt = DateTime.Now,
//            UpdatedAt = DateTime.Now,
//            IsShow = true
//        });
//        _categories.Add(new Category()
//        {
//            Id = 2,
//            Title = "Food",
//            Description = "Delicious and fresh products",
//            Image = "food.jpg",
//            CreatedAt = DateTime.Now,
//            UpdatedAt = DateTime.Now,
//            IsShow = true
//        });
//    }
//    public List<Category> GetAllCategories()
//    {
//        return _categories;
//    }
//}

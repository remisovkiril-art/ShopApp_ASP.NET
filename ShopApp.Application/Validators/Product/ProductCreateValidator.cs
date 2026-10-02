using FluentValidation;
using ShopApplication.DTOs.ProductDTOs;

namespace ShopApplication.Validators.Product;

public class ProductCreateValidator : AbstractValidator<ProductCreateDTO>
{
    public ProductCreateValidator()
    {
        RuleFor(product => product.Name)
            .NotEmpty()
            .WithMessage("Name является обязательным")
            .MaximumLength(100)
            .WithMessage("Name не может быть длиннее 100 символов");

        RuleFor(product => product.Price)
            .GreaterThan(0)
            .WithMessage("Price должен быть больше 0");

        RuleFor(product => product.StockQty)
            .GreaterThanOrEqualTo(0)
            .WithMessage("StockQty не может быть меньше 0");

        RuleFor(product => product.CategoryId)
            .GreaterThan(0)
            .WithMessage("CategoryId должен быть больше 0");
    }
}
using FluentValidation;
using ShopApplication.DTOs.CategoryDTOs;

namespace ShopApplication.Validators.Category;

public class CategoryCreateValidator : AbstractValidator<CategoryCreateDTO>
{
    public CategoryCreateValidator()
    {
        RuleFor(category => category.Name)
            .NotEmpty()
            .WithMessage("Name является обязательным")
            .MaximumLength(30)
            .WithMessage("Name не может быть длиннее 30 символов")
            .Matches(@"^[a-zA-Z0-9_-]+$")
            .WithMessage("Name должен содержать только латинские буквы, цифры и символы - и _");

        RuleFor(category => category.Slug)
            .NotEmpty()
            .WithMessage("Slug является обязательным")
            .Matches(@"^[a-zA-Z0-9_-]+$")
            .WithMessage("Slug должен содержать только латинские буквы, цифры и символы - и _");

        RuleFor(category => category.ParentId)
            .GreaterThan(0)
            .When(category => category.ParentId.HasValue)
            .WithMessage("ParentId должен быть больше 0");
    }
}

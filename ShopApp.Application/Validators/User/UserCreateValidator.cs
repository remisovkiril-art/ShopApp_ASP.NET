using FluentValidation;
using ShopApplication.DTOs.UserDTOs;

namespace ShopApplication.Validators.User;

public class UserCreateValidator : AbstractValidator<UserCreateDTO>
{
    public UserCreateValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("Email является обязательным")
            .EmailAddress()
            .WithMessage("Email имеет некорректный формат");

        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage("Password является обязательным")
            .MinimumLength(5)
            .WithMessage("Password должен содержать минимум 5 символов");
    }
}
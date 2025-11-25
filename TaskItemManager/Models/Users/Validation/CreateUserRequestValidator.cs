using FluentValidation;
using TaskItemManager.Requests.Users;

namespace TaskItemManager.Models.Users.Validation;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(u => u.UserName)
            .NotEmpty()
            .WithMessage(u => $"{nameof(u.UserName)} cannot be empty")
            .MaximumLength(30)
            .WithMessage(u => $"{nameof(u.UserName)} cannot be more than 30 characters");

        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage(u => $"{nameof(u.Email)} cannot be empty")
            .MaximumLength(50)
            .WithMessage(u => $"{nameof(u.Email)} cannot be more than 50 characters");
    }
}

using FluentValidation;
using TaskItemManager.Models.Users.Checkers;
using TaskItemManager.Requests.Users;

namespace TaskItemManager.Models.Users.Validation;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator(IEmailUniqueChecker emailUniqueChecker)
    {
        RuleFor(u => u.UserName)
            .NotEmpty()
            .WithMessage(u => $"{nameof(u.UserName)} cannot be empty")
            .MaximumLength(30)
            .WithMessage(u => $"{nameof(u.UserName)} cannot be more than 30 characters");

        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage(u => $"{nameof(u.Email)} cannot be empty")
            .EmailAddress()
            .WithMessage(u => $"{nameof(u.Email)} must be a valid email")
            .MaximumLength(50)
            .WithMessage(u => $"{nameof(u.Email)} cannot be more than 50 characters")
            .MustAsync(async (email, cancellationToken) =>
            {
                return await emailUniqueChecker.IsUnique(email, cancellationToken);
            })
            .WithMessage(u => $"{nameof(u.Email)} already exists"); ;
    }
}

using FluentValidation;
using TaskItemManager.Repositories.Users;
using TaskItemManager.Requests.Users;

namespace TaskItemManager.Models.Users.Validation;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator(IUsersRepository usersRepository)
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
            .WithMessage(u => $"{nameof(u.Email)} cannot be more than 50 characters")
            .MustAsync(async (email, cancellationToken) =>
            {
                return await usersRepository.EmailExists(email, cancellationToken);
            })
            .WithMessage(u => $"{nameof(u.Email)} already exists"); ;
    }
}

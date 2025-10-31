using FluentValidation;
using TaskItemManager.Requests.TaskItems;

namespace TaskItemManager.Models.TaskItems.Validation;

public class UpdateTaskItemRequestValidator : AbstractValidator<UpdateTaskItemRequest>
{
    public UpdateTaskItemRequestValidator()
    {
        RuleFor(t => t.Title)
            .NotEmpty()
            .WithMessage(t => $"{nameof(t.Title)} cannot be empty")
            .MaximumLength(50)
            .WithMessage(t => $"{nameof(t.Title)} cannot be more than 50 characters");

        RuleFor(t => t.Description)
            .NotEmpty()
            .WithMessage(t => $"{nameof(t.Description)} cannot be empty")
            .MaximumLength(200)
            .WithMessage(t => $"{nameof(t.Description)} cannot be more than 200 characters");
    }
}

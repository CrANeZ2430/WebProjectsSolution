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

        RuleFor(t => t.StartedAt)
            .NotEmpty()
            .WithMessage(t => $"{nameof(t.StartedAt)} cannot be empty")
            .Must(startedAt =>
            {
                return startedAt > DateTime.UtcNow;
            })
            .WithMessage(t => $"{nameof(t.StartedAt)} cannot be earlier than now");

        RuleFor(t => t.DoneAt)
            .NotEmpty()
            .WithMessage(t => $"{nameof(t.DoneAt)} cannot be empty")
            .GreaterThan(t => t.StartedAt)
            .WithMessage(t => $"{nameof(t.DoneAt)} shall be greater than {nameof(t.StartedAt)}")
            .Must(doneAt =>
            {
                return doneAt > DateTime.UtcNow;
            })
            .WithMessage(t => $"{nameof(t.DoneAt)} cannot be earlier than now");
    }
}

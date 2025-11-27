using FluentValidation;
using TaskItemManager.Models.TaskItems.Checkers;
using TaskItemManager.Models.TaskItems.Validation;
using TaskItemManager.Models.Users.Models;
using TaskItemManager.Repositories.Users;
using TaskItemManager.Requests.TaskItems;

namespace TaskItemManager.Models.TaskItems.Models;

public class TaskItem
{
    private TaskItem() { }

    private TaskItem(
        Guid id,
        string title,
        string description,
        bool isCompleted,
        DateTime startedAt,
        DateTime doneAt,
        Guid userId,
        User user)
    {
        Id = id;
        Title = title;
        Description = description;
        IsCompleted = isCompleted;
        StartedAt = startedAt;
        DoneAt = doneAt;
        UserId = userId;
        User = user;
    }

    private TaskItem(
        Guid id, 
        string title, 
        string description, 
        bool isCompleted,
        DateTime startedAt,
        DateTime doneAt,
        Guid userId)
    {
        Id = id;
        Title = title;
        Description = description;
        IsCompleted = isCompleted;
        StartedAt = startedAt;
        DoneAt = doneAt;
        UserId = userId;
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime DoneAt { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }

    public static async Task<TaskItem> Create(
        CreateTaskItemRequest request,
        IUserExistsChecker userExistsChecker,
        CancellationToken cancellationToken = default)
    {
        var validator = new CreateTaskItemRequestValidator(userExistsChecker);
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        return new TaskItem(
            Guid.NewGuid(),
            request.Title,
            request.Description,
            request.IsCompleted,
            request.StartedAt,
            request.DoneAt,
            request.UserId);
    }

    public static TaskItem Create(
        Guid id,
        string title,
        string description,
        bool isCompleted,
        DateTime startedAt,
        DateTime doneAt,
        Guid userId,
        User user)
    {
        return new TaskItem(
            id,
            title,
            description,
            isCompleted,
            startedAt,
            doneAt,
            userId,
            user);
    }

    public async Task Update(
        UpdateTaskItemRequest request, 
        CancellationToken cancellationToken = default)
    {
        var validator = new UpdateTaskItemRequestValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        Title = request.Title;
        Description = request.Description;
        IsCompleted = request.IsCompleted;
    }
}

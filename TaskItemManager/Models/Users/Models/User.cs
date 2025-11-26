using FluentValidation;
using TaskItemManager.Models.TaskItems.Models;
using TaskItemManager.Models.Users.Validation;
using TaskItemManager.Repositories.Users;
using TaskItemManager.Requests.Users;

namespace TaskItemManager.Models.Users.Models;

public class User
{
    private User() { }

    private User(
        Guid id,
        string userName,
        string email,
        DateTime createdAt,
        List<TaskItem> taskItems)
    {
        Id = id;
        UserName = userName;
        Email = email;
        CreatedAt = createdAt;
        _taskItems = taskItems;
    }

    private User(
        string userName,
        string email)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        Email = email;
        CreatedAt = DateTime.UtcNow;
    }

    private readonly List<TaskItem> _taskItems = new();
    public Guid Id { get; private set; }
    public string UserName { get; private set; }
    public string Email { get; private set; }
    public DateTime CreatedAt { get; init; }
    public ICollection<TaskItem> TaskItems => _taskItems;

    public static async Task<User> Create(
        CreateUserRequest request,
        IUsersRepository usersRepository,
        CancellationToken cancellationToken = default)
    {
        var validator = new CreateUserRequestValidator(usersRepository);
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        return new User(
            request.UserName,
            request.Email);
    }

    public static User Create(
        Guid id,
        string userName,
        string email,
        DateTime createdAt,
        List<TaskItem> taskItems)
    {
        return new User(
            id,
            userName,
            email,
            createdAt,
            taskItems);
    }

    public async Task Update(
        UpdateUserRequest request,
        IUsersRepository usersRepository,
        CancellationToken cancellationToken = default)
    {
        var validator = new UpdateUserRequestValidator(usersRepository);
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        UserName = request.UserName;
        Email = request.Email;
    }
}

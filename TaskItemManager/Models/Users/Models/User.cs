using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using TaskItemManager.Models.TaskItems.Models;
using TaskItemManager.Models.Users.Validation;
using TaskItemManager.Requests.Users;

namespace TaskItemManager.Models.Users.Models;

public class User
{
    private User() { }

    private User(
        Guid id,
        string userName,
        string email,
        string passwordHash,
        DateTime createdAt,
        List<TaskItem> taskItems)
    {
        Id = id;
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = createdAt;
        _taskItems = taskItems;
    }

    private User(
        string userName,
        string email,
        string passwordHash)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }

    private readonly List<TaskItem> _taskItems = new();
    public Guid Id { get; private set; }
    public string UserName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public ICollection<TaskItem> TaskItems => _taskItems;

    public static async Task<User> Create(
        CreateUserRequest request, 
        CancellationToken cancellationToken = default)
    {
        var validator = new CreateUserRequestValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        return new User(
            request.UserName,
            request.Email,
            request.PasswordHash);
    }

    public static User Create(
        Guid id,
        string userName,
        string email,
        string passwordHash,
        DateTime createdAt,
        List<TaskItem> taskItems)
    {
        return new User(
            id,
            userName,
            email,
            passwordHash,
            createdAt,
            taskItems);
    }

    public async Task Update(
        UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var validator = new UpdateUserRequestValidator();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        UserName = request.UserName;
        Email = request.Email;
        PasswordHash = request.PasswordHash;
    }
}

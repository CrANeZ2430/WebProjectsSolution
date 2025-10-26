using TaskItemManager.Dtos.Users;
using TaskItemManager.Models.TaskItems;

namespace TaskItemManager.Models.Users;

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

    public static User Create(CreateUserDto dto)
    {
        return new User(
            dto.UserName,
            dto.Email,
            dto.PasswordHash);
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

    public void Update(UpdateUserDto dto)
    {
        UserName = dto.UserName;
        Email = dto.Email;
        PasswordHash = dto.PasswordHash;
    }
}

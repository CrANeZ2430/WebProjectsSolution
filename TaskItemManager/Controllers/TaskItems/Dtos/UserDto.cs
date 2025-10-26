namespace TaskItemManager.Controllers.TaskItems.Dtos;

public record UserDto(
    Guid Id,
    string UserName,
    string Email,
    string PasswordHash,
    DateTime CreatedAt);

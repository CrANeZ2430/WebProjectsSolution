namespace TaskItemManager.Controllers.Users.Dtos;

public record UserDto(
    Guid Id,
    string UserName,
    string Email,
    string PasswordHash,
    DateTime CreatedAt,
    IEnumerable<TaskItemDto> Posts);

namespace TaskItemManager.Dtos.Users;

public record UpdateUserDto(
    string UserName,
    string Email,
    string PasswordHash);

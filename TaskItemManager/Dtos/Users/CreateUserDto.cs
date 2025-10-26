namespace TaskItemManager.Dtos.Users;

public record CreateUserDto(
    string UserName,
    string Email,
    string PasswordHash);
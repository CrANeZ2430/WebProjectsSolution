namespace TaskItemManager.Dtos.Users;

public record CreateUserRequest(
    string UserName,
    string Email,
    string PasswordHash);
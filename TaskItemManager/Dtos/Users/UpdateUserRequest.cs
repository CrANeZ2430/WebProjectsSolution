namespace TaskItemManager.Dtos.Users;

public record UpdateUserRequest(
    string UserName,
    string Email,
    string PasswordHash);

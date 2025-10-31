namespace TaskItemManager.Requests.Users;

public record CreateUserRequest(
    string UserName,
    string Email,
    string PasswordHash);
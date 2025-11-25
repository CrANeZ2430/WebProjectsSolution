namespace TaskItemManager.Requests.Users;

public record UpdateUserRequest(
    string UserName,
    string Email);

namespace TaskItemManager.Controllers.Users.Dtos;

public record UserCreatedResponse(
    Guid Id,
    string UserName,
    string Email,
    DateTime CreatedAt);
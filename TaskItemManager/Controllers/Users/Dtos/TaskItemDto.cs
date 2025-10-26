namespace TaskItemManager.Controllers.Users.Dtos;

public record TaskItemDto(
    Guid Id,
    string Title,
    string Description,
    bool IsCompleted);

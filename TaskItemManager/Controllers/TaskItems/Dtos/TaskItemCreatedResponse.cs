namespace TaskItemManager.Controllers.TaskItems.Dtos;

public record TaskItemCreatedResponse(
    Guid Id,
    string Title,
    string Description,
    bool IsCompleted,
    Guid UserId);

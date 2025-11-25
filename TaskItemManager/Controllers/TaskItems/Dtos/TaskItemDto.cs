namespace TaskItemManager.Controllers.TaskItems.Dtos;

public record TaskItemDto(
    Guid Id,
    string Title,
    string Description,
    bool IsCompleted,
    DateTime StartedAt,
    DateTime DoneAt,
    UserSummaryDto user);
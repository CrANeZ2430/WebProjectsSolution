namespace TaskItemManager.Controllers.Users.Dtos;

public record TaskItemSummaryDto(
    Guid Id,
    string Title,
    string Description,
    bool IsCompleted);

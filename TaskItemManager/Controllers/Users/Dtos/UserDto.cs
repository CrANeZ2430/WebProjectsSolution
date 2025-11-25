namespace TaskItemManager.Controllers.Users.Dtos;

public record UserDto(
    Guid Id,
    string UserName,
    string Email,
    DateTime CreatedAt,
    IEnumerable<TaskItemSummaryDto> TaskItems,
    int TaskItemCount);

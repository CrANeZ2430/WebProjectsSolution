namespace TaskItemManager.Controllers.TaskItems.Dtos;

public record UserSummaryDto(
    Guid Id,
    string UserName,
    string Email,
    string PasswordHash,
    DateTime CreatedAt);

namespace TaskItemManager.Dtos.TaskItems
{
    public record CreateTaskItemDto(
        string Title,
        string Description,
        bool IsCompleted,
        Guid UserId);
}
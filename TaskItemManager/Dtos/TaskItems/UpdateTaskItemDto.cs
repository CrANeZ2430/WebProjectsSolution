namespace TaskItemManager.Dtos.TaskItems
{
    public record UpdateTaskItemDto(
        string Title,
        string Description,
        bool IsCompleted);
}

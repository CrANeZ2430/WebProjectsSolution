namespace TaskItemManager.Dtos.TaskItems
{
    public record UpdateTaskItemRequest(
        string Title,
        string Description,
        bool IsCompleted);
}

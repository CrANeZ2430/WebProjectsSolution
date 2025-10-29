namespace TaskItemManager.Dtos.TaskItems
{
    public record CreateTaskItemRequest(
        string Title,
        string Description,
        bool IsCompleted,
        Guid UserId);
}
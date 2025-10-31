namespace TaskItemManager.Requests.TaskItems
{
    public record CreateTaskItemRequest(
        string Title,
        string Description,
        bool IsCompleted,
        Guid UserId);
}
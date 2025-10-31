namespace TaskItemManager.Requests.TaskItems
{
    public record UpdateTaskItemRequest(
        string Title,
        string Description,
        bool IsCompleted);
}

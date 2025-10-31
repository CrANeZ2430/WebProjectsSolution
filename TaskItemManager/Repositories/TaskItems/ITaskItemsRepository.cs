using TaskItemManager.Models.TaskItems.Models;

namespace TaskItemManager.Repositories.TaskItems
{
    public interface ITaskItemsRepository
    {
        Task<List<TaskItem>> GetTaskItems(CancellationToken cancellationToken = default);
        Task<TaskItem> GetTaskItemById(Guid taskItemId, CancellationToken cancellationToken = default);
        Task AddTaskItem(TaskItem taskItem, CancellationToken cancellationToken = default);
        void UpdateTaskItem(TaskItem taskItem);
        void DeleteTaskItem(TaskItem taskItem);
    }
}

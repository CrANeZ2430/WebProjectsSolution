using Microsoft.EntityFrameworkCore;
using TaskItemManager.Database;
using TaskItemManager.Models.TaskItems;

namespace TaskItemManager.Repositories.TaskItems
{
    public class TaskItemsRepository(TaskItemsDbContext dbContext) : ITaskItemsRepository
    {
        public async Task<List<TaskItem>> GetTaskItems(CancellationToken cancellationToken = default)
        {
            return await dbContext.TaskItems
                .Include(x => x.User)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<TaskItem> GetTaskItemById(Guid taskItemId, CancellationToken cancellationToken = default)
        {
            return await dbContext.TaskItems
                .Include(x => x.User)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == taskItemId, cancellationToken);
        }

        public async Task AddTaskItem(TaskItem taskItem, CancellationToken cancellationToken = default)
        {
            await dbContext.AddAsync(taskItem, cancellationToken);
        }

        public void UpdateTaskItem(TaskItem taskItem)
        {
            dbContext.Update(taskItem);
        }

        public void DeleteTaskItem(TaskItem taskItem)
        {
            dbContext.Remove(taskItem);
        }
    }
}

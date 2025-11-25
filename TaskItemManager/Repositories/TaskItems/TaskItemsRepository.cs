using Microsoft.EntityFrameworkCore;
using TaskItemManager.Database;
using Dapper;
using Npgsql;
using TaskItemManager.Models.TaskItems.Models;
using TaskItemManager.Models.Users.Models;
using TaskItemManager.ExceptionHandling.Exceptions;

namespace TaskItemManager.Repositories.TaskItems
{
    public class TaskItemsRepository(TaskItemsDbContext dbContext, IConfiguration configuration) : ITaskItemsRepository
    {
        public async Task<List<TaskItem>> GetTaskItems(CancellationToken cancellationToken = default)
        {
            //return await dbContext.TaskItems
            //    .Include(x => x.User)
            //    .AsNoTracking()
            //    .ToListAsync(cancellationToken);

            using var connection = new NpgsqlConnection(configuration.GetConnectionString("TaskItemsDb"));
            var sqlQuery = @"select * from ""taskItems"".""TaskItems"" t 
                            inner join ""taskItems"".""Users"" u 
                            on t.""UserId"" = u.""Id""";
            var taskItems = await connection.QueryAsync<TaskItem, User, TaskItem>(sqlQuery, (taskItem, user) =>
            {
                return TaskItem.Create(
                    taskItem.Id,
                    taskItem.Title,
                    taskItem.Description,
                    taskItem.IsCompleted,
                    taskItem.StartedAt,
                    taskItem.DoneAt,
                    taskItem.UserId,
                    user);
            });

            return taskItems.ToList();
        }

        public async Task<TaskItem> GetTaskItemById(Guid taskItemId, CancellationToken cancellationToken = default)
        {
            //return await dbContext.TaskItems
            //    .Include(x => x.User)
            //    .AsNoTracking()
            //    .SingleOrDefaultAsync(x => x.Id == taskItemId, cancellationToken);

            using var connection = new NpgsqlConnection(configuration.GetConnectionString("TaskItemsDb"));
            var sqlQuery = @"select * from ""taskItems"".""TaskItems"" t 
                            inner join ""taskItems"".""Users"" u 
                            on u.""Id"" = t.""UserId"" 
                            where t.""Id"" = @TaskItemId
                            limit 1";
            var taskItems = await connection.QueryAsync<TaskItem, User, TaskItem>(sqlQuery, (taskItem, user) =>
            {
                return TaskItem.Create(
                    taskItem.Id,
                    taskItem.Title,
                    taskItem.Description,
                    taskItem.IsCompleted,
                    taskItem.StartedAt,
                    taskItem.DoneAt,
                    taskItem.UserId,
                    user);
            },
            new { TaskItemId = taskItemId });

            return taskItems.SingleOrDefault() ?? throw new NotFoundException($"{nameof(TaskItem)} cannot be found");
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

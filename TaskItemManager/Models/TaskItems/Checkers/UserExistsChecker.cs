using Microsoft.EntityFrameworkCore;
using TaskItemManager.Database;

namespace TaskItemManager.Models.TaskItems.Checkers;

public class UserExistsChecker(TaskItemsDbContext dbContext) : IUserExistsChecker
{
    public async Task<bool> Exists(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.AnyAsync(u => u.Id == userId);
    }
}

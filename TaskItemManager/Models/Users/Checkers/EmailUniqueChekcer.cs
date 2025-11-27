using Microsoft.EntityFrameworkCore;
using TaskItemManager.Database;

namespace TaskItemManager.Models.Users.Checkers;

public class EmailUniqueChekcer(TaskItemsDbContext dbContext) : IEmailUniqueChecker
{
    public async Task<bool> IsUnique(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.AllAsync(u => u.Email != email);
    }
}

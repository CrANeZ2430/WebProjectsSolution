using Microsoft.EntityFrameworkCore;
using TaskItemManager.Database;
using TaskItemManager.Models.Users;

namespace TaskItemManager.Repositories.Users;

public class UsersRepository(TaskItemsDbContext dbContext) : IUsersRepository
{
    public async Task<List<User>> GetUsers(CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(x => x.TaskItems)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<User> GetUserById(Guid UserId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(x => x.TaskItems)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == UserId);
    }

    public async Task AddUser(User user, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(user, cancellationToken);
    }

    public void UpdateUser(User user)
    {
        dbContext.Update(user);
    }

    public void DeleteUser(User user)
    {
        dbContext.Remove(user);
    }
}

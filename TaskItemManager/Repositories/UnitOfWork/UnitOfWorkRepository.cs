
using TaskItemManager.Database;

namespace TaskItemManager.Repositories.UnitOfWork
{
    public class UnitOfWorkRepository(TaskItemsDbContext dbContext) : IUnitOfWorkRepository
    {
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

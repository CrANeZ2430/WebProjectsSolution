namespace TaskItemManager.Repositories.UnitOfWork
{
    public interface IUnitOfWorkRepository
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

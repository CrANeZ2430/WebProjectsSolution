namespace TaskItemManager.Models.TaskItems.Checkers;

public interface IUserExistsChecker
{
    Task<bool> Exists(Guid userId, CancellationToken cancellationToken = default);
}

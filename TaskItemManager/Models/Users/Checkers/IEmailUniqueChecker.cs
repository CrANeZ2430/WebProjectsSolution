namespace TaskItemManager.Models.Users.Checkers;

public interface IEmailUniqueChecker
{
    Task<bool> IsUnique(string email, CancellationToken cancellationToken = default);
}

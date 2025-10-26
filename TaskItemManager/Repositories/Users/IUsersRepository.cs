using TaskItemManager.Models.Users;

namespace TaskItemManager.Repositories.Users;

public interface IUsersRepository
{
    Task<List<User>> GetUsers(CancellationToken cancellationToken = default);
    Task<User> GetUserById(Guid UserId, CancellationToken cancellationToken = default);
    Task AddUser(User user, CancellationToken cancellationToken = default);
    void UpdateUser(User user);
    void DeleteUser(User user);
}

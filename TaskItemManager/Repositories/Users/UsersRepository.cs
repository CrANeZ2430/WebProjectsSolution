using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TaskItemManager.Database;
using TaskItemManager.Models.TaskItems.Models;
using TaskItemManager.Models.Users.Models;

namespace TaskItemManager.Repositories.Users;

public class UsersRepository(TaskItemsDbContext dbContext, IConfiguration configuration) : IUsersRepository
{
    public async Task<List<User>> GetUsers(CancellationToken cancellationToken = default)
    {
        //return await dbContext.Users
        //    .Include(x => x.TaskItems)
        //    .AsNoTracking()
        //    .ToListAsync(cancellationToken);

        using var connection = new NpgsqlConnection(configuration.GetConnectionString("TaskItemsDb"));

        var userDictionary = new Dictionary<Guid, User>();
        var sqlQuery = @"select * from ""taskItems"".""Users"" u 
                    left join ""taskItems"".""TaskItems"" t 
                    on t.""UserId"" = u.""Id"" 
                    order by u.""Id""";

        var users = await connection.QueryAsync<User, TaskItem, User>(
            sqlQuery,
            (user, taskItem) =>
            {
                if (!userDictionary.TryGetValue(user.Id, out var userEntry))
                {
                    userEntry = User.Create(
                        user.Id,
                        user.UserName,
                        user.Email,
                        user.PasswordHash,
                        user.CreatedAt,
                        user.TaskItems.ToList());
                    userDictionary.Add(user.Id, userEntry);
                }

                if (taskItem != null)
                    userEntry.TaskItems.Add(taskItem);

                return userEntry;
            });

        return userDictionary.Values.ToList();
    }

    public async Task<User> GetUserById(Guid userId, CancellationToken cancellationToken = default)
    {
        //return await dbContext.Users
        //    .Include(x => x.TaskItems)
        //    .AsNoTracking()
        //    .SingleOrDefaultAsync(x => x.Id == userId);

        using var connection = new NpgsqlConnection(configuration.GetConnectionString("TaskItemsDb"));

        User returnUser = null; 
        var sqlQuery = $@"select * from ""taskItems"".""Users"" u 
                        left join ""taskItems"".""TaskItems"" t 
                        on t.""UserId"" = u.""Id"" where u.""Id"" = @UserId 
                        order by u.""Id""";

        var userQuery = await connection.QueryAsync<User, TaskItem, User>(
            sqlQuery, 
            (user, taskItem) => 
        { 
            if (returnUser is null)
            {
                if (taskItem is not null)
                    user.TaskItems.Add(taskItem);
                returnUser = user;
            }
            
            else 
                returnUser.TaskItems.Add(taskItem);

            return user; 
        },
        new { UserId = userId }); 

        return returnUser;
    }

    public async Task<bool> UserExists(Guid userId, CancellationToken cancellationToken = default)
    {
        using var connection = new NpgsqlConnection(configuration.GetConnectionString("TaskItemsDb"));
        var sqlQuery = $@"select 1 from ""taskItems"".""Users"" u 
                        where u.""Id"" = @UserId
                        limit 1";

        return await connection.ExecuteScalarAsync<bool>(sqlQuery, new { UserId = userId });
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

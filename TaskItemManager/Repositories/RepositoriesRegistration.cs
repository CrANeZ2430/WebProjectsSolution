using TaskItemManager.Repositories.TaskItems;
using TaskItemManager.Repositories.UnitOfWork;
using TaskItemManager.Repositories.Users;

namespace TaskItemManager.Repositories
{
    public static class RepositoriesRegistration
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();
            services.AddScoped<ITaskItemsRepository, TaskItemsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
        }
    }
}

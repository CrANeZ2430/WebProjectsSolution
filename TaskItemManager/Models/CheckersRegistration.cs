using TaskItemManager.Models.TaskItems.Checkers;
using TaskItemManager.Models.Users.Checkers;

namespace TaskItemManager.Models;

public static class CheckersRegistration
{
    public static void RegisterCheckers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IEmailUniqueChecker, EmailUniqueChecker>();
        serviceCollection.AddScoped<IUserExistsChecker, UserExistsChecker>();
    }
}

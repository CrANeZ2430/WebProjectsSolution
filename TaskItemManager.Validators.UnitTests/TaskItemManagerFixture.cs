using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskItemManager.Database;
using TaskItemManager.Models.Users.Checkers;

namespace TaskItemManager.Validators.Tests;

public class TaskItemManagerFixture
{
    private readonly string _connectionString = "Host=localhost;Port=5432;Database=taskItemsDb;Username=postgres;Password=greOl8716;";
    public readonly IEmailUniqueChecker EmailUniqueChecker;

    public TaskItemManagerFixture()
    {
        var services = new ServiceCollection();
        services.AddDbContext<TaskItemsDbContext>(options =>
                options.UseNpgsql(_connectionString));
        services.AddScoped<IEmailUniqueChecker, EmailUniqueChecker>();

        EmailUniqueChecker = services.BuildServiceProvider()
                                .GetRequiredService<IEmailUniqueChecker>();
    }
}
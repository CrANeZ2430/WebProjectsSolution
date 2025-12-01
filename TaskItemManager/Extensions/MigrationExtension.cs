using Microsoft.EntityFrameworkCore;
using TaskItemManager.Database;

namespace TaskItemManager.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using TaskItemsDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<TaskItemsDbContext>();

        dbContext.Database.Migrate();
    }
}

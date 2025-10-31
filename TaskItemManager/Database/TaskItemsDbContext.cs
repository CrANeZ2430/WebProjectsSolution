using Microsoft.EntityFrameworkCore;
using TaskItemManager.Models.TaskItems.Models;
using TaskItemManager.Models.Users.Models;

namespace TaskItemManager.Database
{
    public class TaskItemsDbContext(DbContextOptions<TaskItemsDbContext> options) : DbContext(options)
    {
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskItemsDbContext).Assembly);
            modelBuilder.HasDefaultSchema("taskItems");
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }
    }
}

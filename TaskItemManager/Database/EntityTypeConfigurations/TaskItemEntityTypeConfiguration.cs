using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskItemManager.Models.TaskItems.Models;

namespace TaskItemManager.Database.EntityTypeConfigurations;

public class TaskItemEntityTypeConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.IsCompleted)
            .HasDefaultValue(false);

        builder.Property(x => x.StartedAt)
            .IsRequired()
            .HasDefaultValue(DateTime.UtcNow);

        builder.Property(x => x.DoneAt)
            .IsRequired()
            .HasDefaultValue(DateTime.UtcNow);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasOne(ti => ti.User)
            .WithMany(u => u.TaskItems)
            .HasForeignKey(ti => ti.UserId);
    }
}

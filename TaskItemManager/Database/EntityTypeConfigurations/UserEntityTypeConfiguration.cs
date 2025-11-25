using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskItemManager.Models.Users.Models;

namespace TaskItemManager.Database.EntityTypeConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.CreatedAt);

        builder.HasMany(u => u.TaskItems)
            .WithOne(ti => ti.User);

        builder.Navigation(u => u.TaskItems)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;



namespace TaskManager.Infrastructure.EntityConfigurations;

public class TaskConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        //Name And Scheme
        builder.ToTable(nameof(TaskItem));

        //Props
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(t => t.CreatedAt).IsRequired();

        // Identity Key
        builder.HasKey(x => x.Id);

        //Navigation
        builder.HasMany(x => x.TaskLabels).WithOne(x => x.Task).HasForeignKey(x => x.TaskId);
    }
}

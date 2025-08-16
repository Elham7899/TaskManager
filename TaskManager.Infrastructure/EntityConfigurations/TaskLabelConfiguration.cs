using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.EntityConfigurations;

public class TaskLabelConfiguration : IEntityTypeConfiguration<TaskLabel>
{
    public void Configure(EntityTypeBuilder<TaskLabel> builder)
    {
        //Name and Scheme
        builder.ToTable(nameof(TaskLabel));

        //Keys
        builder.HasKey(x => new { x.LabelId, x.TaskId });
    }
}

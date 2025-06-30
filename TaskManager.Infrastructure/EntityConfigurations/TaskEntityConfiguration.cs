using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;



namespace TaskManager.Infrastructure.EntityConfigurations;

public class TaskEntityConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        //Props
        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);

        // Identity Key
        builder.HasKey(x=>x.Id);

        //Navigation
    }
}

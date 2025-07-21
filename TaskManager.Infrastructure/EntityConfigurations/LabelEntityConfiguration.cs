using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.EntityConfigurations;

public class LabelEntityConfiguration : IEntityTypeConfiguration<Label>
{
    public void Configure(EntityTypeBuilder<Label> builder)
    {
        //Name And Scheme
        builder.ToTable(nameof(Label));

        //Identity Key
        builder.HasKey(x => x.Id);

        //Props
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
    }
}

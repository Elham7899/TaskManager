using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        //Name And Schem
        builder.ToTable("Users");

        //Identity
        builder.HasKey(u => u.Id);

        //Props
        builder.Property(u => u.Username).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Password).IsRequired(); 
    }
}

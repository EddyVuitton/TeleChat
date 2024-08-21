using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeleChat.Domain.Models.Entities;

namespace TeleChat.Domain.Models.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Login).IsRequired();
        builder.Property(x => x.Password).IsRequired();
        builder.Property(x => x.Created).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
    }
}
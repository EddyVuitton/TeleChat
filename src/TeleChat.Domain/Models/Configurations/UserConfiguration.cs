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

        builder.HasData(new User
        {
            Id = 1,
            Name = "Konto Demo 1",
            Login = "demo1",
            Password = "739136E95F37FEE4B526F9C20C3E9DA6-11FB2578105BEA2A05F32D9CA5DFD27C", //demo
        });

        builder.HasData(new User
        {
            Id = 2,
            Name = "Konto Demo 2",
            Login = "demo2",
            Password = "739136E95F37FEE4B526F9C20C3E9DA6-11FB2578105BEA2A05F32D9CA5DFD27C", //demo
        });
    }
}
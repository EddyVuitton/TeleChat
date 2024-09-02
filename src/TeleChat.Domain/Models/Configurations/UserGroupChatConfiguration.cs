using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeleChat.Domain.Models.Entities;

namespace TeleChat.Domain.Models.Configurations;

public class UserGroupChatConfiguration : IEntityTypeConfiguration<UserGroupChat>
{
    public void Configure(EntityTypeBuilder<UserGroupChat> builder)
    {
        builder.ToTable("UserGroupChat");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.GroupChatId).IsRequired();
        builder.Property(x => x.Created).IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.GroupChat)
            .WithMany()
            .HasForeignKey(x => x.GroupChatId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasData(new UserGroupChat
        {
            Id = 1,
            UserId = 1,
            GroupChatId = 1
        });

        builder.HasData(new UserGroupChat
        {
            Id = 2,
            UserId = 2,
            GroupChatId = 1
        });
    }
}
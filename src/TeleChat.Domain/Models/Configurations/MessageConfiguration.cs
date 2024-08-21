using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeleChat.Domain.Models.Entities;

namespace TeleChat.Domain.Models.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Message");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Text).IsRequired();
        builder.Property(x => x.MessageTypeId).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.GroupChatId).IsRequired();
        builder.Property(x => x.Created).IsRequired();

        builder
            .HasOne(x => x.MessageType)
            .WithMany()
            .HasForeignKey(x => x.MessageTypeId)
            .OnDelete(DeleteBehavior.NoAction);

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
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeleChat.Domain.Models.Entities;

namespace TeleChat.Domain.Models.Configurations;

public class MessageReactionConfiguration : IEntityTypeConfiguration<MessageReaction>
{
    public void Configure(EntityTypeBuilder<MessageReaction> builder)
    {
        builder.ToTable("MessageReaction");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MessageId).IsRequired();
        builder.Property(x => x.ReactionId).IsRequired();
        builder.Property(x => x.UserId).IsRequired();

        builder
            .HasOne(x => x.Message)
            .WithMany()
            .HasForeignKey(x => x.MessageId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.Reaction)
            .WithMany()
            .HasForeignKey(x => x.ReactionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
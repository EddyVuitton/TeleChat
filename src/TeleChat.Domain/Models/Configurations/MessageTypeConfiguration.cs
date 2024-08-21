using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeleChat.Domain.Models.Entities;

namespace TeleChat.Domain.Models.Configurations;

public class MessageTypeConfiguration : IEntityTypeConfiguration<MessageType>
{
    public void Configure(EntityTypeBuilder<MessageType> builder)
    {
        builder.ToTable("MessageType");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();
    }
}
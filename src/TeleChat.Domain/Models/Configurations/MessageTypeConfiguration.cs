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

        builder.HasData(new List<MessageType>
        {
            new() { Id = 1, Name = "PlainText" },
            new() { Id = 2, Name = "GIF", DefaultStyle = "max-width: 200px; max-height: 200px;" }
        });
    }
}
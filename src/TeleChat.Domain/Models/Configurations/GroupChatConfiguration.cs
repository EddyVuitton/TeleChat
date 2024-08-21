using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeleChat.Domain.Models.Entities;

namespace TeleChat.Domain.Models.Configurations;

public class GroupChatConfiguration : IEntityTypeConfiguration<GroupChat>
{
    public void Configure(EntityTypeBuilder<GroupChat> builder)
    {
        builder.ToTable("GroupChat");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Guid).IsRequired();
        builder.Property(x => x.Created).IsRequired();
    }
}
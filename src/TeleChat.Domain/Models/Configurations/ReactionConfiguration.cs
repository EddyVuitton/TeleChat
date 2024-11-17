using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeleChat.Domain.Models.Entities;

namespace TeleChat.Domain.Models.Configurations;

public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
{
    public void Configure(EntityTypeBuilder<Reaction> builder)
    {
        builder.ToTable("Reaction");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Value).IsRequired();

        builder.HasData(new List<Reaction>
        {
            new() { Id = 1, Value = "👍" },
            new() { Id = 2, Value = "👎" },
            new() { Id = 3, Value = "🧡" },
            new() { Id = 4, Value = "🔥" },
            new() { Id = 5, Value = "🥰" },
            new() { Id = 6, Value = "👏" },
            new() { Id = 7, Value = "😁" },
        });
    }
}
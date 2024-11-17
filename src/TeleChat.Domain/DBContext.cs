using Microsoft.EntityFrameworkCore;
using TeleChat.Domain.Models.Entities;
using TeleChat.Domain.Models.Configurations;

namespace TeleChat.Domain;

public class DBContext(DbContextOptions<DBContext> options) : DbContext(options)
{
    public DbSet<User> User => Set<User>();
    public DbSet<Message> Message => Set<Message>();
    public DbSet<MessageType> MessageType => Set<MessageType>();
    public DbSet<GroupChat> GroupChat => Set<GroupChat>();
    public DbSet<UserGroupChat> UserGroupChat => Set<UserGroupChat>();
    public DbSet<Reaction> Reaction => Set<Reaction>();
    public DbSet<MessageReaction> MessageReaction => Set<MessageReaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new MessageConfiguration());
        modelBuilder.ApplyConfiguration(new MessageTypeConfiguration());
        modelBuilder.ApplyConfiguration(new GroupChatConfiguration());
        modelBuilder.ApplyConfiguration(new UserGroupChatConfiguration());
        modelBuilder.ApplyConfiguration(new ReactionConfiguration());
        modelBuilder.ApplyConfiguration(new MessageReactionConfiguration());
    }
}
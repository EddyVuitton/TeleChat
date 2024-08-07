using Microsoft.EntityFrameworkCore;
using TeleChat.Domain.Entities;

namespace TeleChat.Domain.Context;

public class DBContext(DbContextOptions<DBContext> options) : DbContext(options)
{
    public DbSet<User> User => Set<User>();
    public DbSet<Message> Message => Set<Message>();
    public DbSet<MessageType> MessageType => Set<MessageType>();
    public DbSet<Receiver> Receiver => Set<Receiver>();
    public DbSet<ReceiverType> ReceiverType => Set<ReceiverType>();
}
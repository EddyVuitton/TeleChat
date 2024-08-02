using Microsoft.EntityFrameworkCore;
using TeleChat.Domain.Entities;

namespace TeleChat.Domain.Context;

public class DBContext(DbContextOptions<DBContext> options) : DbContext(options)
{
    public DbSet<User> User => Set<User>();
}
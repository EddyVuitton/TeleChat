using TeleChat.Domain.Context;

namespace TeleChat.WebAPI.Services;

public class SeedDataService(DBContext context)
{
    private readonly DBContext _context = context;


}
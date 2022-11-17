using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class SpiffoDbContext : DbContext
{
    public SpiffoDbContext(DbContextOptions options) : base(options)
    {
    }
}
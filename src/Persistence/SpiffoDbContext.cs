using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence;

public class SpiffoDbContext : DbContext
{
    public DbSet<GameServer> GameServers => Set<GameServer>();
    public DbSet<GameServerRole> GameServerRoles => Set<GameServerRole>();
    
    public SpiffoDbContext(DbContextOptions options) : base(options)
    {
    }
}
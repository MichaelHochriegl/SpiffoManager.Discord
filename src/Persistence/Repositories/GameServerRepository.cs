using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence.Repositories;

public class GameServerRepository : IGameServerRepository
{
    private readonly SpiffoDbContext _dbContext;

    public GameServerRepository(SpiffoDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<bool> IsGameServerNameAlreadyUsedAsync(string serverName, CancellationToken ct = default)
    {
        return await _dbContext.GameServers
            .AnyAsync(g => g.ServerName == serverName, ct);
    }
    
    public async Task<bool> IsGameServerInstallAlreadyUsedAsync(string serverInstallPath, CancellationToken ct = default)
    {
        return await _dbContext.GameServers
            .AnyAsync(g => g.ServerInstallPath == serverInstallPath, ct);
    }
    
    public async Task AddAsync(GameServer gameServer, CancellationToken ct = default)
    {
        _dbContext.GameServers.Add(gameServer);
        await _dbContext.SaveChangesAsync(ct);
    }
}
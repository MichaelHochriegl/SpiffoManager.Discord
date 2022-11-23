using Persistence.Entities;

namespace Persistence.Repositories;

public interface IGameServerRepository
{
    Task<bool> IsGameServerNameAlreadyUsedAsync(string serverName, CancellationToken ct = default);
    Task<bool> IsGameServerInstallAlreadyUsedAsync(string serverInstallPath, CancellationToken ct = default);
    Task AddAsync(GameServer gameServer, CancellationToken ct = default);
}
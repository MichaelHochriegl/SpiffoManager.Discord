using Persistence.Entities;

namespace Persistence.Repositories;

/// <summary>
/// Repository for handling <see cref="GameServer"/> specific DB actions
/// </summary>
public interface IGameServerRepository
{
    /// <summary>
    /// Checks if a <see cref="GameServer"/> with the given <paramref name="serverName"/> is already present
    /// </summary>
    /// <param name="serverName">The name of the server to check against</param>
    /// <param name="ct">A <see cref="CancellationToken"/> to observe while waiting for the task to complete</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.
    /// The task result contains true if a <see cref="GameServer"/> with the given <param name="serverName"></param> is already present;
    /// otherwise, false.
    /// </returns>
    Task<bool> IsGameServerNameAlreadyUsedAsync(string serverName, CancellationToken ct = default);
    
    /// <summary>
    /// Checks if a <see cref="GameServer"/> with the given <paramref name="serverInstallPath"/> is already present
    /// </summary>
    /// <param name="serverInstallPath">The path of the server to check against</param>
    /// <param name="ct">A <see cref="CancellationToken"/> to observe while waiting for the task to complete</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.
    /// The task result contains true if a <see cref="GameServer"/> with the given <param name="serverInstallPath"></param> is already present;
    /// otherwise, false.
    /// </returns>
    Task<bool> IsGameServerInstallAlreadyUsedAsync(string serverInstallPath, CancellationToken ct = default);
    
    /// <summary>
    /// Adds the given <paramref name="gameServer"/> to the DB
    /// </summary>
    /// <param name="gameServer">The <see cref="GameServer"/> instance to add</param>
    /// <param name="ct">A <see cref="CancellationToken"/> to observe while waiting for the task to complete</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task AddAsync(GameServer gameServer, CancellationToken ct = default);
}
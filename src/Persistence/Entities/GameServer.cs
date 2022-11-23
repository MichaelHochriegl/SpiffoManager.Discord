namespace Persistence.Entities;

/// <summary>
/// Entity that represents a gameserver
/// </summary>
public class GameServer
{
    /// <summary>
    /// Unique ID of the entity
    /// </summary>
    public ulong Id { get; set; }
    
    /// <summary>
    /// Name of the server with which it can be accessed
    /// </summary>
    public string ServerName { get; set; } = "pzserver";
    
    /// <summary>
    /// Path where the actual LGSM instance lies
    /// </summary>
    public string ServerInstallPath { get; set; } = "../";

    private GameServer()
    {
        
    }

    public GameServer(string serverName, string serverInstallPath)
    {
        ServerName = serverName;
        ServerInstallPath = serverInstallPath;
    }
}
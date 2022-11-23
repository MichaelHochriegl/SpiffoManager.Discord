namespace Persistence.Entities;

public class GameServer
{
    public ulong Id { get; set; }
    public string ServerName { get; set; } = "pzserver";
    public string ServerInstallPath { get; set; } = "../";
}
using System.ComponentModel;

namespace Discord.Enums;

/// <summary>
/// Available commands to control the gameserver
/// </summary>
public enum ServerCommands
{
    [Description("Starts the server.")]
    Start,
    [Description("Immediately stops the server, no warning for players!")]
    ImmediateStop,
    [Description("Immediately restarts the server, no warning for players!")]
    ImmediateRestart,
    [Description("Updates the server. Stops it first if it's still running and starts it back up afterwards.")]
    ImmediateUpdate,
    [Description("Gracefully stops the server by giving the players time to get save and disconnect.")]
    GracefulStop,
    [Description("Gracefully restarts the server by giving the players time to get save and disconnect.")]
    GracefulRestart,
    [Description("Shows wether the server is running or stopped.")]
    Status
}
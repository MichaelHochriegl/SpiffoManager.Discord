using ShellAccess.Scripts;

namespace ShellAccess.ServerCommands;

/// <summary>
/// Immediately restarts the server giving the players no time to get in a save position
/// </summary>
public class ImmediateRestartCommand : ServerCommandBase
{
    private const string Command = "restart";

    public ImmediateRestartCommand(IRunner scriptRunner, string gameServerName = "pzserver") 
        : base(scriptRunner, new LgsmScript(gameServerName, Command))
    {
    }

    public override async Task<ServerCommandResult> ExecuteCommandAsync()
    {
        var result = await base.ExecuteCommandAsync();

        switch (result.State)
        {
            case ServerCommandResultState.Successful:
                result.Message = "Server successfully restarted.";
                break;
            case ServerCommandResultState.Error:
                result.Message = "Server could not be restarted! Try stopping and starting it manually.";
                break;
            default:
                result.Message = $"State {result.State} is not defined for this command!";
                break;
        }

        return result;
    }
}
using ShellAccess.Scripts;

namespace ShellAccess.ServerCommands;

/// <summary>
/// Gracefully restarts the server by giving the players a warning and time to get in a save position
/// </summary>
public class GracefulRestartCommand : ServerCommandBase
{
    private const string Script = "graceful_restart.sh";

    public GracefulRestartCommand(IRunner scriptRunner, string gameServerName) 
        : base(scriptRunner, new CustomScript(Script, new []{ gameServerName, "30" }))
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
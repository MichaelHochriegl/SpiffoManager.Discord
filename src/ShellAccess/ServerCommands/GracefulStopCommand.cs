using ShellAccess.Scripts;

namespace ShellAccess.ServerCommands;

/// <summary>
/// Gracefully stops the server by giving the players a warning and time to get in a save position
/// </summary>
public class GracefulStopCommand : ServerCommandBase
{
    private const string Script = "graceful_stop.sh";

    public GracefulStopCommand(IRunner scriptRunner, string gameServerName) 
        : base(scriptRunner, new CustomScript(Script, new []{ gameServerName, "30" }))
    {
    }

    public override async Task<ServerCommandResult> ExecuteCommandAsync()
    {
        var result = await base.ExecuteCommandAsync();

        switch (result.State)
        {
            case ServerCommandResultState.Successful:
                result.Message = "Server successfully stopped.";
                break;
            case ServerCommandResultState.Error:
                result.Message = "Server is already stopped!";
                break;
            default:
                result.Message = $"State {result.State} is not defined for this command!";
                break;
        }

        return result;
    }
}
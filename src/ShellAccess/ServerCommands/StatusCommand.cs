using ShellAccess.Scripts;

namespace ShellAccess.ServerCommands;

/// <summary>
/// Queries the server's running status
/// </summary>
public class StatusCommand : ServerCommandBase
{
    private const string Command = "monitor";

    public StatusCommand(IRunner scriptRunner, string gameServerName = "pzserver") 
        : base(scriptRunner, new LgsmScript(gameServerName, Command))
    {
    }

    public override async Task<ServerCommandResult> ExecuteCommandAsync()
    {
        var result = await base.ExecuteCommandAsync();

        switch (result.State)
        {
            case ServerCommandResultState.Successful:
                result.Message = "Server is running!";
                break;
            case ServerCommandResultState.Error:
                result.Message = "Server is stopped!";
                break;
            default:
                result.Message = $"State {result.State} is not defined for this command!";
                break;
        }

        return result;
    }
}
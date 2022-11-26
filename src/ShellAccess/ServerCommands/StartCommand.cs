using ShellAccess.Scripts;

namespace ShellAccess.ServerCommands;

/// <summary>
/// Immediately starts the server
/// </summary>
public class StartCommand : ServerCommandBase
{
    private const string Command = "start";

    public StartCommand(IRunner scriptRunner, string gameServerName = "pzserver") 
        : base(scriptRunner, new LgsmScript(gameServerName, Command))
    {
    }

    public override async Task<ServerCommandResult> ExecuteCommandAsync()
    {
        var result = await base.ExecuteCommandAsync();

        switch (result.State)
        {
            case ServerCommandResultState.Successful:
                result.Message = "Server successfully started.";
                break;
            case ServerCommandResultState.Error:
                result.Message = "Server is already running!";
                break;
            default:
                result.Message = $"State {result.State} is not defined for this command!";
                break;
        }

        return result;
    }
}
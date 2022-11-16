using ShellAccess.Scripts;

namespace ShellAccess.ServerCommands;

/// <summary>
/// Immediately starts the server
/// </summary>
public class StartCommand : ServerCommandBase
{
    private const string Command = "start";
    private static readonly LgsmScript Script = new(Command);
    
    public StartCommand(IRunner scriptRunner) : base(scriptRunner, Script)
    {
    }

    protected override async Task<ServerCommandResult> ExecuteCommandAsync()
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
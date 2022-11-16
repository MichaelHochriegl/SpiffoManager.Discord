using ShellAccess.Scripts;

namespace ShellAccess.ServerCommands;

/// <summary>
/// Immediately stops the server giving the players no time to get in a save position
/// </summary>
public class ImmediateStopCommand : ServerCommandBase
{
    private const string Command = "stop";
    private static readonly LgsmScript Script = new(Command);
    
    public ImmediateStopCommand(IRunner scriptRunner) : base(scriptRunner, Script)
    {
    }

    protected override async Task<ServerCommandResult> ExecuteCommandAsync()
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
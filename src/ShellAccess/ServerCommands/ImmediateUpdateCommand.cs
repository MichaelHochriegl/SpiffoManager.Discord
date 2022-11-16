using ShellAccess.Scripts;

namespace ShellAccess.ServerCommands;

/// <summary>
/// Immediately checks for updates and if any shuts down the server giving the players no time to get in a save position
/// </summary>
public class ImmediateUpdateCommand : ServerCommandBase
{
    private const string Command = "update";
    private static readonly LgsmScript Script = new(Command);
    
    public ImmediateUpdateCommand(IRunner scriptRunner) : base(scriptRunner, Script)
    {
    }

    public override async Task<ServerCommandResult> ExecuteCommandAsync()
    {
        var result = await base.ExecuteCommandAsync();

        switch (result.State)
        {
            case ServerCommandResultState.Successful:
                result.Message = "Server successfully updated.";
                break;
            case ServerCommandResultState.Error:
                result.Message = "Server could not be updated!";
                break;
            default:
                result.Message = $"State {result.State} is not defined for this command!";
                break;
        }

        return result;
    }
}
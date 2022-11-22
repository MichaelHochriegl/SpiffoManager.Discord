using ShellAccess.Scripts;

namespace ShellAccess.ServerCommands;

/// <summary>
/// Gracefully restarts the server by giving the players a warning and time to get in a save position
/// </summary>
public class GetPlayersCommand : ServerCommandBase
{
    private const string ScriptPath = "../get_players.sh";
    private static readonly CustomScript Script = new(ScriptPath);
    
    public GetPlayersCommand(IRunner scriptRunner) : base(scriptRunner, Script)
    {
    }

    public override async Task<ServerCommandResult> ExecuteCommandAsync()
    {
        var result = await base.ExecuteCommandAsync();

        switch (result.State)
        {
            case ServerCommandResultState.Successful:
                result.Message = $"Current players online: {result.RawMessage}";
                break;
            case ServerCommandResultState.Error:
                result.Message = "Server could not retrieve the playercount!";
                break;
            default:
                result.Message = $"State {result.State} is not defined for this command!";
                break;
        }

        return result;
    }
}
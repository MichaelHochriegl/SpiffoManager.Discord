namespace ShellAccess.ServerCommands;

/// <summary>
/// Execution result of the <see cref="ServerCommandBase"/>
/// </summary>
public class ServerCommandResult
{
    public int? ExitCode { get; }
    public string? Message { get; set; }
    public string? RawMessage { get; }
    public ServerCommandResultState State { get; }
    
    public ServerCommandResult(int? exitCode, string? message = null, string? rawMessage = null)
    {
        ExitCode = exitCode;
        Message = message;
        RawMessage = rawMessage;
        State = ExitCodeToState(exitCode);
    }

    private ServerCommandResultState ExitCodeToState(int? exitCode)
    {
        var state = ServerCommandResultState.Fatal;
        switch (exitCode)
        {
            case 0:
                state = ServerCommandResultState.Successful;
                break;
            case 1:
                state = ServerCommandResultState.Fatal;
                break;
            case 2:
                state = ServerCommandResultState.Error;
                break;
            case 3:
                state = ServerCommandResultState.Warning;
                break;
            case null:
                state = ServerCommandResultState.Info;
                break;
        }

        return state;
    }
}

public enum ServerCommandResultState
{
    Successful,
    Fatal,
    Error,
    Warning,
    Info
}
using ShellAccess.Scripts;

namespace ShellAccess.ServerCommands;

/// <summary>
/// Base class for any server command
/// </summary>
public abstract class ServerCommandBase
{
    private readonly IRunner _scriptRunner;
    private readonly ScriptBase _script;

    protected ServerCommandBase(IRunner scriptRunner, ScriptBase script)
    {
        _scriptRunner = scriptRunner;
        _script = script;
    }

    protected virtual async Task<ServerCommandResult> ExecuteCommandAsync()
    {
        var scriptResult = await _scriptRunner.ExecuteScript(_script);
        var result = new ServerCommandResult(exitCode: scriptResult.CommandResult.ExitCode, rawMessage: scriptResult.CliMessage);
        return result;
    }
}
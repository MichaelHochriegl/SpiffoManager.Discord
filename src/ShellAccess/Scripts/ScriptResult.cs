using CliWrap;

namespace ShellAccess.Scripts;
/// <summary>
/// Result of the <see cref="ScriptBase"/> execution
/// </summary>
/// <param name="CliMessage">Raw message returned by the executed script</param>
/// <param name="CommandResult">CLI result with return code of the executed script</param>
public record ScriptResult(string CliMessage, CommandResult CommandResult);
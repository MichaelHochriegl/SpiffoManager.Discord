namespace ShellAccess.Scripts;

/// <summary>
/// Runner to execute scripts
/// </summary>
public interface IRunner
{
    /// <summary>
    /// Executes the given <see cref="ScriptBase"/>
    /// </summary>
    /// <param name="script">The <see cref="ScriptBase"/> to execute</param>
    /// <returns><see cref="ScriptResult"/> of the execution</returns>
    Task<ScriptResult> ExecuteScript(ScriptBase script);
}
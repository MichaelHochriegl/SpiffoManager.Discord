namespace ShellAccess.Scripts;

/// <summary>
/// Base representation of a script
/// </summary>
public abstract class ScriptBase
{
    /// <summary>
    /// The script to execute with the absolute path
    /// </summary>
    public string Script { get; }
    
    /// <summary>
    /// Arguments that will get passed to the <see cref="Script"/>
    /// </summary>
    public IEnumerable<string> Arguments { get; }

    protected ScriptBase(string script, IEnumerable<string> arguments)
    {
        Script = script;
        Arguments = arguments;
    }

    protected ScriptBase(string script, string singleArgument) : this(script, new[] { singleArgument })
    {
    }
}
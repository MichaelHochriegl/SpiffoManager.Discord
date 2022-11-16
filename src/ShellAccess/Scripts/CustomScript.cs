namespace ShellAccess.Scripts;

/// <summary>
/// Allows for the execution of custom scripts
/// </summary>
public class CustomScript : ScriptBase
{
    public CustomScript(string script, IEnumerable<string> arguments) : base(script, arguments)
    {
    }

    public CustomScript(string script) : base(script, Array.Empty<string>())
    {
    }
}
namespace ShellAccess.Scripts;

/// <summary>
/// Representation of a LGSM command
/// </summary>
public class LgsmScript : ScriptBase
{
    
    public LgsmScript(string script, IEnumerable<string> arguments) : base(script, arguments)
    {
    }

    public LgsmScript(string script, string singleArgument) : base(script, singleArgument)
    {
    }
}
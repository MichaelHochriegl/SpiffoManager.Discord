namespace ShellAccess.Scripts;

/// <summary>
/// Representation of a LGSM command
/// </summary>
public class LgsmScript : ScriptBase
{
    private new const string Script = "pzserver";

    public LgsmScript(IEnumerable<string> arguments) : base(Script, arguments)
    {
    }

    public LgsmScript(string singleArgument) : base(Script, singleArgument)
    {
    }
}
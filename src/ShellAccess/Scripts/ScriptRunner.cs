using System.Text;
using CliWrap;

namespace ShellAccess.Scripts;

public class ScriptRunner : IRunner
{
    public string ScriptDir { get; set; } = "../";

    public async Task<ScriptResult> ExecuteScript(ScriptBase script)
    {
        var scriptPath = string.Join(Path.DirectorySeparatorChar, ScriptDir, script.Script);
        var resultBuilder = new StringBuilder();
        var resultErrorBuilder = new StringBuilder();
        var cmd = Cli.Wrap(scriptPath)
            .WithArguments(script.Arguments)
            .WithValidation(CommandResultValidation.None)
            .WithStandardOutputPipe(PipeTarget.ToStringBuilder(resultBuilder))
            .WithStandardErrorPipe(PipeTarget.ToStringBuilder(resultErrorBuilder));

        var cliResult = await cmd.ExecuteAsync();
        var message = string.IsNullOrEmpty(resultErrorBuilder.ToString())
            ? resultBuilder.ToString()
            : resultErrorBuilder.ToString();
        var result = new ScriptResult(message, cliResult);
        return result;
    }
}
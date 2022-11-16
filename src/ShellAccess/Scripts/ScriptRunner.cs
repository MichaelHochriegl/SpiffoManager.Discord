using System.Text;
using CliWrap;

namespace ShellAccess.Scripts;

public class ScriptRunner : IRunner
{
    public async Task<ScriptResult> ExecuteScript(ScriptBase script)
    {
        var resultBuilder = new StringBuilder();
        var resultErrorBuilder = new StringBuilder();
        var cmd = Cli.Wrap(script.Script)
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
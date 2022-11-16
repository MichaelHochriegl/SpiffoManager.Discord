using ShellAccess.ServerCommands;

namespace Discord.Extensions;

/// <summary>
/// Extensions for <see cref="Embed"/>
/// </summary>
public static class EmbedExtensions
{
    /// <summary>
    /// Converts the given <paramref name="message"/> to an <see cref="Embed"/> with an error representation
    /// </summary>
    /// <param name="message">The message to convert</param>
    /// <param name="commandName">The discord command this message refers to</param>
    /// <returns>The error <see cref="Embed"/> representation of the <paramref name="message"/></returns>
    public static Embed ToErrorEmbed(this string message, string? commandName = null)
    {
        return message.ToEmbed(commandName, Color.Red);
    }
    
    /// <summary>
    /// Converts the given <paramref name="message"/> to an <see cref="Embed"/> with a info representation
    /// </summary>
    /// <param name="message">The message to convert</param>
    /// <param name="commandName">The discord command this message refers to</param>
    /// <returns>The info <see cref="Embed"/> representation of the <paramref name="message"/></returns>
    public static Embed ToInfoEmbed(this string message, string? commandName = null)
    {
        return message.ToEmbed(commandName, Color.Blue);
    }
    
    /// <summary>
    /// Converts the given <paramref name="message"/> to an <see cref="Embed"/> with a warning representation
    /// </summary>
    /// <param name="message">The message to convert</param>
    /// <param name="commandName">The discord command this message refers to</param>
    /// <returns>The warning <see cref="Embed"/> representation of the <paramref name="message"/></returns>
    public static Embed ToWarningEmbed(this string message, string? commandName = null)
    {
        return message.ToEmbed(commandName, Color.Orange);
    }
    
    /// <summary>
    /// Converts the given <paramref name="message"/> to an <see cref="Embed"/> with a success representation
    /// </summary>
    /// <param name="message">The message to convert</param>
    /// <param name="commandName">The discord command this message refers to</param>
    /// <returns>The success <see cref="Embed"/> representation of the <paramref name="message"/></returns>
    public static Embed ToSuccessEmbed(this string message, string? commandName = null)
    {
        return message.ToEmbed(commandName, Color.Green);
    }

    /// <summary>
    /// Converts the given <paramref name="result"/> to an <see cref="Embed"/>
    /// </summary>
    /// <param name="result"><see cref="ServerCommandResult"/> to convert</param>
    /// <param name="commandName">The discord command this message refers to</param>
    /// <returns>The <see cref="Embed"/> representation of the <paramref name="result"/></returns>
    public static Embed ToEmbed(this ServerCommandResult result, string commandName)
    {
        var builder = new EmbedBuilder();
        builder.WithAuthor("Spiffo.Manager")
            .WithFooter($"Response for command '{commandName}'");

        Color color;
        var description = result.Message ?? result.RawMessage ?? $"Fatal error parsing message!";
        switch (result.State)
        {
            case ServerCommandResultState.Successful:
                color = Color.Green;
                break;
            case ServerCommandResultState.Fatal:
                color = Color.DarkRed;
                break;
            case ServerCommandResultState.Error:
                color = Color.Red;
                break;
            case ServerCommandResultState.Warning:
                color = Color.Orange;
                break;
            case ServerCommandResultState.Info:
                color = Color.Blue;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(result));
        }

        builder.WithColor(color);
        builder.WithDescription(description);

        return builder.Build();
    }

    /// <summary>
    /// Converts the given <paramref name="message"/> to an <see cref="Embed"/> with the provided <see cref="Color"/>
    /// </summary>
    /// <param name="message">The message to convert</param>
    /// <param name="commandName">The discord command this message refers to</param>
    /// <returns>The <see cref="Embed"/> representation of the <paramref name="message"/> with the given <paramref name="color"/></returns>
    public static Embed ToEmbed(this string message, string? commandName = null, Color color = default)
    {
        var builder = new EmbedBuilder();
        builder.WithAuthor("Spiffo.Manager");
        if (commandName is not null) builder.WithFooter($"Response for command '{commandName}'");
        builder.WithColor(color);
        builder.WithDescription(message);

        return builder.Build();
    }
}
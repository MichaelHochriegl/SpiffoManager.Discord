using Discord.Enums;
using Discord.Extensions;
using Discord.Interactions;
using JetBrains.Annotations;
using ShellAccess.Scripts;
using ShellAccess.ServerCommands;

namespace Discord.Modules;

/// <summary>
/// Discord interaction module for commands targeting a PZ gameserver
/// </summary>
public class ServerModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IRunner _scriptRunner;

    public ServerModule(IRunner scriptRunner)
    {
        _scriptRunner = scriptRunner;
    }

    /// <summary>
    /// Issues the given <see cref="ServerCommands"/> to the gameserver
    /// </summary>
    /// <param name="input">The <see cref="ServerCommands"/> to execute</param>
    [UsedImplicitly]
    [SlashCommand("pzserver", "Issuing commands to the Project Zomboid Gameserver.")]
    public async Task ExecuteServerCommand(ServerCommands input)
    {
        if (Context.User.HasRole(920780251846553610) is false)
        {
            await RespondAsync(embed: $"You are not allowed to execute this command!".ToEmbed());
            return;
        }
        var command = BuildCommand(input);
        
        await RespondAsync(embed: $"Executing command {input}, please wait...".ToEmbed());
        var result = await command.ExecuteCommandAsync();
        await FollowupAsync(embed: result.ToEmbed(input.ToString()));
    }

    private ServerCommandBase BuildCommand(ServerCommands input)
    {
        ServerCommandBase? command = input switch
        {
            ServerCommands.Start => new StartCommand(_scriptRunner),
            ServerCommands.ImmediateStop => new ImmediateStopCommand(_scriptRunner),
            ServerCommands.ImmediateRestart => new ImmediateRestartCommand(_scriptRunner),
            ServerCommands.ImmediateUpdate => new ImmediateUpdateCommand(_scriptRunner),
            ServerCommands.GracefulStop => new GracefulStopCommand(_scriptRunner),
            ServerCommands.GracefulRestart => new GracefulRestartCommand(_scriptRunner),
            ServerCommands.Status => new StatusCommand(_scriptRunner),
            _ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
        };

        return command;
    }
}
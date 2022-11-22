using Discord.Enums;
using Discord.Extensions;
using Discord.Interactions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using ShellAccess.Scripts;
using ShellAccess.ServerCommands;

namespace Discord.Modules;

/// <summary>
/// Discord interaction module for commands targeting a PZ gameserver
/// </summary>
public class ServerModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IRunner _scriptRunner;
    private readonly ulong _roleId;

    public ServerModule(IRunner scriptRunner, IConfiguration configuration)
    {
        _scriptRunner = scriptRunner;
        _roleId = configuration.GetValue<ulong>("DiscordBotSettings:GameserverRoleId");
    }

    /// <summary>
    /// Issues the given <see cref="ServerCommands"/> to the gameserver
    /// </summary>
    /// <param name="input">The <see cref="ServerCommands"/> to execute</param>
    [UsedImplicitly]
    [SlashCommand("pzserver", "Issuing commands to the Project Zomboid Gameserver.")]
    public async Task ExecuteServerCommand(ServerCommands input)
    {
        if (Context.User.HasRole(_roleId) is false)
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
            ServerCommands.PlayerCount => new GetPlayersCommand(_scriptRunner),
            _ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
        };

        return command;
    }
}
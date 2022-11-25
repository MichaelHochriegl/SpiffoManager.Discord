using Discord.Enums;
using Discord.Extensions;
using Discord.Interactions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Entities;
using Persistence.Repositories;
using ShellAccess.Scripts;
using ShellAccess.ServerCommands;

namespace Discord.Modules;

/// <summary>
/// Discord interaction module for commands targeting a PZ gameserver
/// </summary>
public class ServerModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IRunner _scriptRunner;
    private readonly IServiceProvider _serviceProvider;
    private readonly ulong _roleId;

    public ServerModule(IRunner scriptRunner, IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _scriptRunner = scriptRunner;
        _serviceProvider = serviceProvider;
        _roleId = configuration.GetValue<ulong>("DiscordBotSettings:GameserverRoleId");
    }

    /// <summary>
    /// Issues the given <see cref="ServerCommands"/> to the gameserver
    /// </summary>
    /// <param name="serverName">The <see cref="GameServer"/> server name to target the command at.</param>
    /// <param name="input">The <see cref="ServerCommands"/> to execute</param>
    [UsedImplicitly]
    [SlashCommand("pzserver", "Issuing commands to the Project Zomboid Gameserver.")]
    public async Task ExecuteServerCommand(string serverName, ServerCommands input)
    {
        if (Context.User.HasRole(_roleId) is false)
        {
            await RespondAsync(embed: $"You are not allowed to execute this command!".ToEmbed());
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var gameServerRepository = scope.ServiceProvider.GetRequiredService<IGameServerRepository>();
        var gameServerPresent = await gameServerRepository.IsGameServerNameAlreadyUsedAsync(serverName);

        if (gameServerPresent is false)
        {
            await RespondAsync(embed: $"No gameserver with name '{serverName}' known!".ToErrorEmbed());
            return;
        }

        var gameServer = await gameServerRepository.GetByNameOrDefaultAsync(serverName);

        if (gameServer is null)
        {
            await RespondAsync(embed: $"Could not get gameserver with name '{serverName}', is it not set up?".ToErrorEmbed());
            return;
        }
        
        var command = BuildCommand(gameServer, input);
        
        await RespondAsync(embed: $"Executing command {input}, please wait...".ToEmbed());
        var result = await command.ExecuteCommandAsync();
        await FollowupAsync(embed: result.ToEmbed(input.ToString()));
    }

    private ServerCommandBase BuildCommand(GameServer gameServer, ServerCommands input)
    {
        _scriptRunner.ScriptDir = gameServer.ServerInstallPath;
        ServerCommandBase? command = input switch
        {
            ServerCommands.Start => new StartCommand(_scriptRunner, gameServer.ServerName),
            ServerCommands.ImmediateStop => new ImmediateStopCommand(_scriptRunner, gameServer.ServerName),
            ServerCommands.ImmediateRestart => new ImmediateRestartCommand(_scriptRunner, gameServer.ServerName),
            ServerCommands.ImmediateUpdate => new ImmediateUpdateCommand(_scriptRunner, gameServer.ServerName),
            ServerCommands.GracefulStop => new GracefulStopCommand(_scriptRunner, gameServer.ServerName),
            ServerCommands.GracefulRestart => new GracefulRestartCommand(_scriptRunner, gameServer.ServerName),
            ServerCommands.Status => new StatusCommand(_scriptRunner, gameServer.ServerName),
            ServerCommands.PlayerCount => new GetPlayersCommand(_scriptRunner, gameServer.ServerName),
            _ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
        };

        return command;
    }
}
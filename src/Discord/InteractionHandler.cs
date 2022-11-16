using System.Reflection;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Discord;

/// <summary>
/// Handler for <see cref="Discord"/> interactions
/// </summary>
public class InteractionHandler
{
    private readonly ILogger<InteractionHandler> _logger;
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _handler;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;

    public InteractionHandler(ILogger<InteractionHandler> logger, DiscordSocketClient client,
        InteractionService handler,
        IServiceProvider services,
        IConfiguration config)
    {
        _logger = logger;
        _client = client;
        _handler = handler;
        _services = services;
        _configuration = config;
    }
    
    /// <summary>
    /// Initializes the bot by executing the steps:
    /// * Registering all commands once bot is ready
    /// * Wiring up the Discord logging
    /// * Registering all <see cref="InteractionModuleBase{T}"/>
    /// * Wiring up the interaction handler
    /// </summary>
    public async Task InitializeAsync()
    {
        // Process when the client is ready, so we can register our commands.
        _client.Ready += ReadyAsync;
        _handler.Log += LogAsync;

        // Add the public modules that inherit InteractionModuleBase<T> to the InteractionService
        await _handler.AddModulesAsync(Assembly.GetAssembly(typeof(InteractionHandler)), _services);

        // Process the InteractionCreated payloads to execute Interactions commands
        _client.InteractionCreated += HandleInteraction;
    }

    private async Task LogAsync(LogMessage log)
    {
        var severity = log.Severity switch
        {
            LogSeverity.Critical => LogEventLevel.Fatal,
            LogSeverity.Error => LogEventLevel.Error,
            LogSeverity.Warning => LogEventLevel.Warning,
            LogSeverity.Info => LogEventLevel.Information,
            LogSeverity.Verbose => LogEventLevel.Verbose,
            LogSeverity.Debug => LogEventLevel.Debug,
            _ => LogEventLevel.Information
        };
        Log.Write(severity, log.Exception, "[{Source}] {Message}", log.Source, log.Message);
        await Task.CompletedTask;
    }

    private async Task ReadyAsync()
    {
        // Context & Slash commands can be automatically registered, but this process needs to happen after the client enters the READY state.
        var guildId = _configuration.GetValue<ulong>("DiscordBotSettings:GuildId");
        _logger.LogDebug("Registering the commands to Guild '{GuildId}'", guildId);
        await _handler.RegisterCommandsToGuildAsync(guildId, true);
    }
    
    private async Task HandleInteraction(SocketInteraction interaction)
    {
        try
        {
            // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules.
            var context = new SocketInteractionContext(_client, interaction);

            // Execute the incoming command.
            var result = await _handler.ExecuteCommandAsync(context, _services);

            if (!result.IsSuccess)
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        _logger.LogWarning("Unmet precondition: {reason}", result.ErrorReason);
                        await context.Channel.SendMessageAsync($"Unmet Precondition! Reason: {result.ErrorReason}");
                        break;
                    case InteractionCommandError.Unsuccessful:
                        await context.Channel.SendMessageAsync($"Failed! Reason: {result.ErrorReason}");
                        break;
                    case InteractionCommandError.UnknownCommand:
                        _logger.LogWarning("Unknown Command! Reason: {reason}", result.ErrorReason);
                        await context.Channel.SendMessageAsync($"Unknown Command!");
                        break;
                    case InteractionCommandError.ConvertFailed:
                        _logger.LogWarning("Converting Failed! Reason: {reason}", result.ErrorReason);
                        await context.Channel.SendMessageAsync($"Converting Failed! Reason: {result.ErrorReason}");
                        break;
                    case InteractionCommandError.BadArgs:
                        _logger.LogWarning("Bad Arguments! Reason: {reason}", result.ErrorReason);
                        await context.Channel.SendMessageAsync($"Bad Arguments! Reason: {result.ErrorReason}");
                        break;
                    case InteractionCommandError.ParseFailed:
                        _logger.LogWarning("Parsing Failed! Reason: {reason}", result.ErrorReason);
                        await context.Channel.SendMessageAsync($"Parsing Failed! Reason: {result.ErrorReason}");
                        break;
                    case InteractionCommandError.Exception:
                        _logger.LogWarning("Exception! Reason: {reason}", result.ErrorReason);
                        await context.Channel.SendMessageAsync($"Exception while executing command! Reason: {result.ErrorReason}");
                        break;
                    case null:
                        _logger.LogWarning("Result Error was null!");
                        break;
                    default:
                        _logger.LogWarning("Could not determine error state! Reason: {reason}", result.ErrorReason);
                        await context.Channel.SendMessageAsync("Ups, something went terribly wrong!");
                        break;
                }
        }
        catch
        {
            // If Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
            // response, or at least let the user know that something went wrong during the command execution.
            if (interaction.Type is InteractionType.ApplicationCommand)
                await interaction.GetOriginalResponseAsync()
                    .ContinueWith(async (msg) => await msg.Result.DeleteAsync());
        }
    }
}
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Discord;

/// <summary>
/// Centralized <see cref="IServiceCollection"/> registration
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the required services for Discord
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register the services into</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> instance to retrieve app configuration</param>
    /// <returns>The <see cref="IServiceCollection"/> after the service registration</returns>
    public static IServiceCollection AddDiscord(this IServiceCollection services, IConfiguration configuration)
    {
        var socketConfig = new DiscordSocketConfig()
        {
            LogLevel = LogSeverity.Verbose,
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers,
            AlwaysDownloadUsers = true,
        };

        services
            .AddSingleton(socketConfig)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
            .AddSingleton<InteractionHandler>();
        
        return services;
    }
}
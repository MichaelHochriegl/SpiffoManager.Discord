using Discord;
using Discord.WebSocket;
using Serilog;
using Serilog.Events;

namespace Bot;

public class Worker : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly DiscordSocketClient _client;
    private readonly InteractionHandler _handler;

    public Worker(
        IConfiguration configuration,
        DiscordSocketClient client,
        InteractionHandler handler)
    {
        _configuration = configuration;
        _client = client;
        _handler = handler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _client.Log += LogAsync;
        
        await _handler.InitializeAsync();

        await _client.LoginAsync(TokenType.Bot, _configuration["DiscordBotSettings:Token"]);
        await _client.StartAsync();
        
        await Task.Delay(Timeout.Infinite, stoppingToken);
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
}
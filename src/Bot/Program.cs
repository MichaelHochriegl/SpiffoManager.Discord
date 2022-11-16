using Bot;
using Discord;
using Serilog;
using ShellAccess;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile("appsettings.Development.json", optional: true, true)
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try
{
    Log.Information("Spiffo.Manager starting up...");
    var host = Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureServices(services =>
        {
            services.AddShellAccess();
            services.AddDiscord(configuration);
            services.AddHostedService<Worker>();
        })
        .Build();

    await host.RunAsync();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Spiffo.Manager failed to start!");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
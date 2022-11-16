using Microsoft.Extensions.DependencyInjection;
using ShellAccess.Scripts;

namespace ShellAccess;

/// <summary>
/// Centralized <see cref="IServiceCollection"/> registration
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the required services for the shell access
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register the services into</param>
    /// <returns>The <see cref="IServiceCollection"/> after the service registration</returns>
    public static IServiceCollection AddShellAccess(this IServiceCollection services)
    {
        services.AddSingleton<IRunner, ScriptRunner>();
        
        return services;
    }
}
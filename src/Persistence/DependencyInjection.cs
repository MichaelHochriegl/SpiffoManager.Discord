using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SpiffoDbContext>(options =>
        {
            var connectionStringSelector = configuration
                .GetValue<string>("ApplicationSettings:UsedConnectionString");
            var connectionString = configuration.GetConnectionString(connectionStringSelector);
            options.UseSqlite(connectionString, opt => 
                opt.MigrationsAssembly(typeof(SpiffoDbContext).Assembly.FullName));
        });

        return services;
    }
}
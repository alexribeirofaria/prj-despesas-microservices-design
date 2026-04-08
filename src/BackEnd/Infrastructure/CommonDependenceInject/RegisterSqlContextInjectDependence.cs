using Infrastructure.DatabaseContexts;
using Infrastructure.DatabaseContexts.Strategy;
using Infrastructure.DatabaseContexts.Strategy.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Infrastructure.CommonInjectDependence;

public static class RegisterSqlContextInjectDependence
{
    public static IServiceCollection ConfigureRegisterSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration.GetValue<string>("DatabaseProvider");

        IDatabaseProviderStrategy databaseProviderStrategy;

        switch (provider)
        {
            case "MySql":
                databaseProviderStrategy = new MySqlProviderStrategy();
                break;

            case "SqlServer":
                databaseProviderStrategy = new SqlServerProviderStrategy();
                break;

            default:
                throw new InvalidOperationException($"Database provider '{provider}' is not supported.");
        }

        services.AddSingleton<IDatabaseProviderStrategy>(databaseProviderStrategy);

        services.AddScoped<DbContext>(sp =>
        {
            var options = sp.GetRequiredService<DbContextOptions<RegisterContext>>();
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            return new RegisterContext(options, databaseProviderStrategy, loggerFactory);
        });

        string connectionString = configuration.GetConnectionString("MySqlConnectionString")
           ?? configuration.GetConnectionString("SqlConnectionString")
           ?? throw new Exception("Connection string não encontrada no appsettings.json.");

        services.AddDbContext<DbContext>((sp, options) =>
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

            if (provider == "MySql")
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
            }
            else if (provider == "SqlServer")
            {
                options.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
            }

            options.UseLoggerFactory(loggerFactory);
            options.UseLazyLoadingProxies();
        });

        return services;
    }
}

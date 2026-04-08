using AuthService.Services;
using AuthService.Services.Interfaces;
using AuthService.Settings;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthService.Register;

public static class ConsulRegistrationExtensions
{
    public static IServiceCollection AddConsulSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ServiceSettings>(configuration.GetSection(nameof(ServiceSettings)));

        services.AddSingleton<IConsulClient>(p =>
        {
            var serviceSettings = p.GetRequiredService<IOptions<ServiceSettings>>().Value;
            return new ConsulClient(config => config.Address = new Uri(serviceSettings.ServiceDiscoveryAddress));
        });

        services.AddSingleton<IMachineIdentifierProvider, MachineIdentifierProvider>();
        services.AddSingleton<IHashService, Sha256HashService>();
        services.AddSingleton<IConsulRegister, ConsulRegistrar>();

        return services;
    }

    public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
    {
        var registrar = app.ApplicationServices.GetRequiredService<IConsulRegister>();
        var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

        var serviceSettings = app.ApplicationServices.GetRequiredService<IOptions<ServiceSettings>>().Value;
        registrar.RegisterAsync(serviceSettings).Wait();

        lifetime.ApplicationStopping.Register(() =>
        {
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>()
                .CreateLogger("Consul");

            logger.LogInformation("Application stopping, unregistering from Consul");
        });

        return app;
    }
}
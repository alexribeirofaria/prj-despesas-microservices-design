using AuthService.Services.Interfaces;
using AuthService.Settings;
using Consul;
using Microsoft.Extensions.Logging;

namespace AuthService.Services;

public class ConsulRegistrar : IConsulRegister
{
    private readonly IConsulClient _client;
    private readonly ILogger<ConsulRegistrar> _logger;
    private readonly IMachineIdentifierProvider _identifierProvider;
    private readonly IHashService _hashService;

    public ConsulRegistrar(
        IConsulClient client,
        ILogger<ConsulRegistrar> logger,
        IMachineIdentifierProvider identifierProvider,
        IHashService hashService)
    {
        _client = client;
        _logger = logger;
        _identifierProvider = identifierProvider;
        _hashService = hashService;
    }

    public async Task RegisterAsync(ServiceSettings settings)
    {
        var id = _identifierProvider.GetIdentifier();
        var hash = _hashService.Compute(id);

        var instanceId = $"{settings.ServiceName}-{hash}";

        var registration = new AgentServiceRegistration
        {
            ID = instanceId,
            Name = settings.ServiceName,
            Address = settings.ServiceHost,
            Port = settings.ServicePort,
            Tags = new[] { settings.ServiceName }
        };

        _logger.LogInformation("Registering service {ServiceName} in Consul", settings.ServiceName);

        await _client.Agent.ServiceDeregister(registration.ID);
        await _client.Agent.ServiceRegister(registration);
    }
}
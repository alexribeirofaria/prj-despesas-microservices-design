using Consul;
using Ocelot.Logging;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Consul.Interfaces;

namespace api_gateway.Configuration
{
    public class ConsulServiceBuilder : DefaultConsulServiceBuilder
    {
        private readonly IConfiguration _configuration;

        public ConsulServiceBuilder(IHttpContextAccessor contextAccessor,
                                    IConsulClientFactory clientFactory,
                                    IOcelotLoggerFactory loggerFactory,
                                    IConfiguration configuration) 
            : base(contextAccessor, clientFactory, loggerFactory)
        {
            _configuration = configuration;
        }

        protected override string GetDownstreamHost(ServiceEntry entry, Node node)
        {
            var scheme = _configuration["GlobalConfiguration:ServiceDiscoveryProvider:Scheme"] ?? "http";
            Console.WriteLine($"Resolving Downstream Host: {scheme}://{entry.Service.Address}:{entry.Service.Port}");
            return $"{scheme}://{entry.Service.Address}:{entry.Service.Port}";
        }

    }
}

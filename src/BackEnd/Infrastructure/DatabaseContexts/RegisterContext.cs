using Infrastructure.DatabaseContexts.Strategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DatabaseContexts;

public class RegisterContext : BaseContext<RegisterContext>
{
    public RegisterContext(
        DbContextOptions<RegisterContext> options,
        IDatabaseProviderStrategy providerStrategy,
        ILoggerFactory loggerFactory) :
        base(options, providerStrategy, loggerFactory)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

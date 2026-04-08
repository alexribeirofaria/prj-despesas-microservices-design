using Domain.Core.ValueObject;
using Infrastructure.DatabaseContexts.Strategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Mapping;
using Domain.Entities;

namespace Infrastructure.DatabaseContexts;

public abstract class BaseContext<TContext> : DbContext where TContext : DbContext
{
    protected readonly IDatabaseProviderStrategy ProviderStrategy;
    private readonly ILoggerFactory _loggerFactory;
    public BaseContext<DbContext> Context { get; }

    protected BaseContext(DbContextOptions<TContext> options, IDatabaseProviderStrategy providerStrategy, ILoggerFactory loggerFactory = null)
        : base(options)
    {
        ProviderStrategy = providerStrategy;
        _loggerFactory = loggerFactory;
    }

    public DbSet<Acesso> Acesso { get; set; }
    public DbSet<Categoria> Categoria { get; set; }
    public DbSet<Despesa> Despesa { get; set; }
    public DbSet<Receita> Receita { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<ImagemPerfilUsuario> ImagemPerfilUsuario { get; set; }
    public DbSet<Lancamento> Lancamento { get; set; }

    public DbSet<TipoCategoria> TipoCategoria { get; set; }
    public DbSet<PerfilUsuario> PerfilUsuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AcessoMap(ProviderStrategy));
        modelBuilder.ApplyConfiguration(new DespesaMap(ProviderStrategy));
        modelBuilder.ApplyConfiguration(new ReceitaMap(ProviderStrategy));
        modelBuilder.ApplyConfiguration(new UsuarioMap(ProviderStrategy));
        modelBuilder.ApplyConfiguration(new ImagemPerfilUsuarioMap(ProviderStrategy));
        modelBuilder.ApplyConfiguration(new LancamentoMap(ProviderStrategy));
        modelBuilder.ApplyConfiguration(new TipoCategoriaMap(ProviderStrategy));
        modelBuilder.ApplyConfiguration(new PerfilUsuarioMap(ProviderStrategy));
        modelBuilder.ApplyConfiguration(new CategoriaMap(ProviderStrategy));
        base.OnModelCreating(modelBuilder);
    }
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_loggerFactory != null)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
        optionsBuilder.UseLazyLoadingProxies();
        base.OnConfiguring(optionsBuilder);
    }
}

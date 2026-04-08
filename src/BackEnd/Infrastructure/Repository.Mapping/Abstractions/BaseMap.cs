using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Infrastructure.DatabaseContexts.Strategy;

namespace Repository.Mapping.Abstractions;

public abstract class BaseMap<TEntity, TKey> : IEntityTypeConfiguration<TEntity> where TEntity : class
{
    private readonly IDatabaseProviderStrategy _providerStrategy;

    protected BaseMap(IDatabaseProviderStrategy providerStrategy)
    {
        _providerStrategy = providerStrategy;
    }

    protected IDatabaseProviderStrategy ProviderStrategy => _providerStrategy;

    // Configura a entidade e aplica as especificidades do provedor de banco de dados
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        ConfigureEntity(builder);  // Método abstrato para configurar a entidade
        _providerStrategy.ApplyProviderSpecifics(builder);  // Aplica configurações específicas do banco de dados
    }

    // Método abstrato para configuração específica da entidade
    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);

    // Método utilitário para configurar Guid nullable
    protected PropertyBuilder<Guid?> ConfigureNullableGuid<TEntityType>(EntityTypeBuilder<TEntityType> builder, string propertyName)
        where TEntityType : class
    {
        return _providerStrategy.ConfigureNullableGuid(builder, propertyName);
    }
}

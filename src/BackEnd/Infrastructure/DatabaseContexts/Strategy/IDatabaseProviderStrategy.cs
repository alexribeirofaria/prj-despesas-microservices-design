using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DatabaseContexts.Strategy;

public interface IDatabaseProviderStrategy
{
    void ApplyProviderSpecifics<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class;
    PropertyBuilder<Guid?> ConfigureNullableGuid<TEntityType>(EntityTypeBuilder<TEntityType> builder, string propertyName) where TEntityType : class;
}

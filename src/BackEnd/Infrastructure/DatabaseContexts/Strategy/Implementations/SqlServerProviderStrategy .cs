using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DatabaseContexts.Strategy.Implementations;

public class SqlServerProviderStrategy : IDatabaseProviderStrategy
{
    public void ApplyProviderSpecifics<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
        var idProperty = typeof(TEntity).GetProperty("Id");
        if (idProperty != null)
        {
            builder.Property<Guid>("Id")
                   .IsRequired();
            builder.HasKey("Id");
        }

        foreach (var prop in typeof(TEntity).GetProperties())
        {
            if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
            {
                builder.Property(prop.Name)
                       .HasColumnType("datetime")
                       .HasDefaultValueSql("GetDate()")
                       .IsRequired();
            }
        }
    }

    public PropertyBuilder<Guid?> ConfigureNullableGuid<TEntityType>(EntityTypeBuilder<TEntityType> builder, string propertyName) where TEntityType : class
    {
        return builder.Property<Guid?>(propertyName)
                      .HasColumnType("varchar(36)")
                      .HasConversion(
                          v => v.HasValue ? v.Value.ToString() : null,
                          v => v != null ? new Guid(v) : (Guid?)null
                      );
    }
}

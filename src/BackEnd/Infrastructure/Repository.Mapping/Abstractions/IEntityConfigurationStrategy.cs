using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Mapping.Abstractions;

public interface IEntityConfigurationStrategy<TEntity> where TEntity : class
{
    void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
}

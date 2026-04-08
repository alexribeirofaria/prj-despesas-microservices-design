using Domain.Core.ValueObject;
using Infrastructure.DatabaseContexts.Strategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Mapping.Abstractions;

namespace Repository.Mapping;

public class TipoCategoriaMap : BaseMap<TipoCategoria, int>
{
    public TipoCategoriaMap(IDatabaseProviderStrategy providerStrategy)
        : base(providerStrategy) { }

    protected override void ConfigureEntity(EntityTypeBuilder<TipoCategoria> builder)
    {
        builder.ToTable("TipoCategoria");
        builder.HasKey(tc => tc.Id);

        builder.Property(tc => tc.Name).IsRequired();

        builder.HasData(
            new TipoCategoria(TipoCategoria.CategoriaType.Despesa),
            new TipoCategoria(TipoCategoria.CategoriaType.Receita)
        );
    }
}

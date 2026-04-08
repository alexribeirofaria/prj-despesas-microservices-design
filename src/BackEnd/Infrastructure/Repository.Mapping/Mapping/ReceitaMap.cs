using Domain.Entities;
using Infrastructure.DatabaseContexts.Strategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Mapping.Abstractions;

namespace Repository.Mapping;

public class ReceitaMap : BaseMap<Receita, Guid>
{
    public ReceitaMap(IDatabaseProviderStrategy providerStrategy)
        : base(providerStrategy) { }

    protected override void ConfigureEntity(EntityTypeBuilder<Receita> builder)
    {
        builder.ToTable(nameof(Receita));

        builder.Property(r => r.Descricao)
               .IsRequired(false)
               .HasMaxLength(100);

        builder.HasOne(r => r.Categoria)
               .WithMany(c => c.Receitas)
               .HasForeignKey(r => r.CategoriaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(r => r.Valor)
               .HasColumnType("decimal(10, 2)")
               .HasDefaultValue(0);

        builder.HasOne(r => r.Usuario)
               .WithMany()
               .HasForeignKey(r => r.UsuarioId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}

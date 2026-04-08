using Domain.Entities;
using Infrastructure.DatabaseContexts.Strategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Mapping.Abstractions;

namespace Repository.Mapping;
public class CategoriaMap : BaseMap<Categoria, Guid>
{
    public CategoriaMap(IDatabaseProviderStrategy providerStrategy)
        : base(providerStrategy) { }

    protected override void ConfigureEntity(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable(nameof(Categoria));

        builder.Property(c => c.Descricao)
               .IsRequired(false)
               .HasMaxLength(100);

        builder.Property(c => c.UsuarioId)
               .IsRequired();

        builder.Property(c => c.TipoCategoriaId)
               .IsRequired();

        // Relacionamentos
        builder.HasOne(c => c.Usuario)
               .WithMany(u => u.Categorias)
               .HasForeignKey(c => c.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.TipoCategoria)
               .WithMany()
               .HasForeignKey(c => c.TipoCategoriaId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

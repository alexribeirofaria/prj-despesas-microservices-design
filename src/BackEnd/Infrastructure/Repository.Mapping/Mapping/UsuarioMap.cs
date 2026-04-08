using Domain.Entities;
using Infrastructure.DatabaseContexts.Strategy;
using Infrastructure.DatabaseContexts.Strategy.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Mapping.Abstractions;

namespace Repository.Mapping;

public class UsuarioMap : BaseMap<Usuario, Guid>
{
    public UsuarioMap(IDatabaseProviderStrategy providerStrategy)
        : base(providerStrategy) { }

    protected override void ConfigureEntity(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable(nameof(Usuario));

        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Email).IsUnique(true);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(50);

        builder.Property(u => u.Nome).HasMaxLength(50).IsRequired();
        builder.Property(u => u.SobreNome).HasMaxLength(50).IsRequired();
        builder.Property(u => u.Telefone).HasMaxLength(15).IsRequired(false);

        builder.HasOne(u => u.PerfilUsuario).WithMany();

        builder.Property(u => u.Profile)
               .IsRequired(false)
               .HasColumnType(ProviderStrategy switch
               {
                   MySqlProviderStrategy _ => "LONGTEXT",
                   SqlServerProviderStrategy _ => "NVARCHAR(MAX)",
                   OracleProviderStrategy _ => "CLOB",
                   _ => "TEXT"
               });
    }
}

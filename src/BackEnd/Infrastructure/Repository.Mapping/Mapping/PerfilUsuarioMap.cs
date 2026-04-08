using Domain.Core.ValueObject;
using Infrastructure.DatabaseContexts.Strategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Mapping.Abstractions;

namespace Repository.Mapping;

public class PerfilUsuarioMap : BaseMap<PerfilUsuario, int>
{
    public PerfilUsuarioMap(IDatabaseProviderStrategy providerStrategy)
        : base(providerStrategy) { }

    protected override void ConfigureEntity(EntityTypeBuilder<PerfilUsuario> builder)
    {
        builder.ToTable("PerfilUsuario");
        builder.HasKey(pu => pu.Id);
        builder.Property(pu => pu.Name).IsRequired();

        builder.HasData(
            new PerfilUsuario(PerfilUsuario.Perfil.Admin),
            new PerfilUsuario(PerfilUsuario.Perfil.User)
        );
    }
}
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Mapping.Abstractions;

namespace Repository.Mapping.Strategies;

public class MySqlEntityConfigurationStrategy : IEntityConfigurationStrategy<Acesso>
{
    public void ConfigureEntity(EntityTypeBuilder<Acesso> builder)
    {
        builder.ToTable("ControleAcesso");

        // Login
        builder.Property(ca => ca.Login)
               .IsRequired()
               .HasMaxLength(100);
        builder.HasIndex(ca => ca.Login).IsUnique();

        // Relacionamento com Usuario
        builder.Property(ca => ca.UsuarioId).IsRequired();

        // Tokens
        builder.Property(ca => ca.RefreshToken)
               .HasDefaultValue(null)
               .IsRequired(false);

        builder.Property(ca => ca.RefreshTokenExpiry)
               .HasDefaultValue(null)
               .IsRequired(false);

        // External Provider / Id
        builder.Property(ca => ca.ExternalProvider);
        builder.HasIndex(ca => ca.ExternalProvider).IsUnique(false);

        builder.Property(ca => ca.ExternalId);
        builder.HasIndex(ca => ca.ExternalId).IsUnique();
    }
}

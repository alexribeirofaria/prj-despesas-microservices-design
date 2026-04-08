using AuthService.Settings;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AuthService.Register;

public static class IdentityServerExtensions
{
    public static IServiceCollection AddIdentityServerSettings(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityServerSettings>(configuration.GetSection("IdentityServerSettings"));

        var serviceProvider = services.BuildServiceProvider();
        var identityServerOptions = serviceProvider.GetRequiredService<IOptions<IdentityServerSettings>>().Value;

        services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme).AddIdentityServerAuthentication(options =>
        {
            options.Authority = identityServerOptions.Authority;
            options.ApiName = identityServerOptions.ApiName;
            options.ApiSecret = identityServerOptions.ApiSecret;
            options.RequireHttpsMetadata = identityServerOptions.RequireHttpsMetadata;
            options.LegacyAudienceValidation = true;
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Bearer",
                new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());

            options.AddPolicy("role-categoria", policy =>
                    policy.RequireRole("User", "Admin"));
            options.AddPolicy("role-despesa", policy =>
                policy.RequireRole("User", "Admin"));
            options.AddPolicy("role-receita", policy =>
                policy.RequireRole("User", "Admin"));
        });
        return services;
    }
}

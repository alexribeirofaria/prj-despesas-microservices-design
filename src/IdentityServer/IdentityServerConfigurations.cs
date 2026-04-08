using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.IdentityModel.Tokens;

namespace STS.Server;

internal class IdentityServerConfigurations
{
    static readonly string[] USER_CLAIMS = { "openid", "profile", "email", "userid", "role" };
    const string SCOPE_NAME = "sts-scope";
    const string DISPLAY_NAME_SCOPE = "STS.Server.Scope";
    const string SECRET = "sts-secret";
    const string CLIENT_ID = "client-microservices";
    const string CLIENT_NAME = "Microservices Applications";

    public static IEnumerable<IdentityResource> GetIdentityResource()
    {
        return new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
        return new List<ApiResource>()
        {
            // Configura o recurso da API com o Audience correto
            new ApiResource("api-gateway", "API Gateway", USER_CLAIMS)  // Altere o nome da API para 'api-gateway'
            {
                ApiSecrets =
                {
                    new Secret("sts-secret-api-gateway".Sha256())  // Altere o segredo para 'api-gateway'
                },
                Scopes = { "sts-scope" },  // Escopos que a API irá utilizar
                AllowedAccessTokenSigningAlgorithms = { SecurityAlgorithms.RsaSha256 }
            }
        };
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return
        [
            new ApiScope()
            {
                Name = SCOPE_NAME,
                DisplayName = DISPLAY_NAME_SCOPE,
                UserClaims = USER_CLAIMS
            }
        ];
    }


    public static IEnumerable<Client> GetClients()
    {
        return
        [
            new Client()
            {
                ClientId = CLIENT_ID,
                ClientName = CLIENT_NAME,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                ClientSecrets =
                {
                    new Secret(SECRET.Sha256())
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.AccessTokenAudience,
                    SCOPE_NAME
                },
                RedirectUris = { "http://internal:5055/signin-oidc", "https://internal:7199/signin-oidc" },
                RefreshTokenExpiration = TokenExpiration.Sliding,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowOfflineAccess = true,
            }
        ];
    }
}

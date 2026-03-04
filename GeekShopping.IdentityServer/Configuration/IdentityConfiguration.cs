using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace GeekShopping.IdentityServer.Configuration
{
    public static class IdentityConfiguration
    {
        public const string Admin = "Admin";
        public const string Client = "Client";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope> {
    new ApiScope("geek_shopping", "GeekShopping API")
};

        public static IEnumerable<ApiResource> ApiResources =>
          new List<ApiResource>
          {
        new ApiResource("geek_shopping", "Geek Shopping API")
        {
            Scopes = { "geek_shopping" },
            // Garante que o Token venha como um JWT legível
            UserClaims = { "role", "name" }
        }
          };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedScopes = { "read", "write", "profile" }
                },
                new Client
{
    ClientId = "geek_shopping",
    ClientSecrets = { new Secret("secret".Sha256()) },
    // Mude para este GrantType para ser mais abrangente
    AllowedGrantTypes = GrantTypes.Code,
    AlwaysIncludeUserClaimsInIdToken = true,
    RedirectUris = { "https://localhost:5164/signin-oidc" },
    PostLogoutRedirectUris = { "https://localhost:5164/signout-callback-oidc" },
    RequirePkce = true,
    AllowOfflineAccess = true,
    AllowedScopes = { "openid", "profile", "email", "geek_shopping" }
}
            };
    }
}
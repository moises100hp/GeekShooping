using Duende.IdentityServer.Models;

namespace GeekShopping.IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
            // O nome que seu microserviço usará para validar o acesso
            new ApiScope("api_minha_app", "Acesso ao Microserviço")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
            new Client
            {
                ClientId = "client_id_exemplo",
                // Usado para comunicação entre serviços (Machine-to-Machine)
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("senha_super_secreta".Sha256()) },
                AllowedScopes = { "api_minha_app" }
            }
            };
    }
}

using Duende.IdentityServer.Models;

namespace Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
                {
                    new ApiScope ()
                    {
                        Name = "test",
                        DisplayName = "full access"
                    }
                };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("shopApi") { Scopes = { "test" } },
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
                {
                    new Client
                    {
                        ClientId = "hello",
                        ClientName = "Hello",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        ClientSecrets = new List<Secret>{ new Secret("Secret".Sha512())},
                        AllowedScopes = new List<string> {"test"},
                    }
                };
    }
}
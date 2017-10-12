using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Boilerplate.OAuth
{
    /// <summary>
    /// Used to configure in-memory settings for testing
    /// </summary>
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> ApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("oauth-boilerplate", "OAuth Boilerplate")
            };
        }

        public static IEnumerable<Client> Clients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "consoleapptoken",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("super-secret-key".Sha256())
                    },
                    AllowedScopes = { "oauth-boilerplate" }
                },
                new Client
                {
                    ClientId = "mvctoken",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("super-duper-secret-key".Sha256())
                    },
                    RedirectUris           = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "oauth-boilerplate"
                    },
                    AllowOfflineAccess = true,
                    RequireConsent = false
                }
            };
        }
    }
}

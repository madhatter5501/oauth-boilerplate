﻿using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Boilerplate.OAuth
{
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
                    ClientId = "consoleapp",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("super-secret-key".Sha256())
                    },
                    AllowedScopes = { "oauth-boilerplate" }
                }
            };
        }
    }
}
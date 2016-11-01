using System;
using System.Collections.Generic;
using IdentityServer4.Models;
using System.Linq;
using System.Security.Claims;
using IdentityServer4.Services.InMemory;

namespace Test.Identity
{
    public class IdentityConfiguration
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "client",

                    ClientId = "client",

                    // no interactive user, use the clientid/secret and/or username/password for authentication
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,

                    // secret for authentication
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = new List<string>
                    {
                        "api",
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Roles.Name
                    }
                }
            };
        }

        public static IEnumerable<Scope> GetScopes()
        {
            return new List<Scope>
            {
                new Scope
                {
                    Name = "api",
                    Description = "Identity API",
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("name", alwaysInclude: true),
                        new ScopeClaim("given_name", alwaysInclude: true),
                        new ScopeClaim("family_name", alwaysInclude: true),
                        new ScopeClaim("email", alwaysInclude: true),
                        new ScopeClaim("role", alwaysInclude: true),
                        new ScopeClaim("website", alwaysInclude: true),
                    }
                },
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.Roles
            };
        }

        public static List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "1",
                    Username = "georgeo@slalom.com",
                    Password = "N0$k!!ng",
                    Claims = new List<Claim>
                    {
                        new Claim("website", "asdf"),
                        new Claim("email", "georgeo@slalom.com"),
                        new Claim("given_name", "georgeo@slalom.com")
                    }
                },
                new InMemoryUser
                {
                    Subject = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }
    }
}
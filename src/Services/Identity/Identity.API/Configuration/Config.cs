using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace AnteyaSidOnContainers.Services.Identity.API.Configuration
{
    public class Config
    {
        /// <summary>
        /// Defines apis in the system
        /// </summary>
        /// <returns>List of api resources</returns>
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("orders", "Orders Service"),
                new ApiResource("basket", "Basket Service"),
                new ApiResource("marketing", "Marketing Service"),
                new ApiResource("locations", "Locations Service")
            };
        }

        /// <summary>
        /// http://docs.identityserver.io/en/release/configuration/resources.html
        /// </summary>
        /// <returns>List of identity resources</returns>
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        /// <summary>
        /// Client want to access resources (aka scopes)
        /// </summary>
        /// <param name="clientsUrl"></param>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientsUrl)
        {
            return new List<Client>
            {
                 //JavaScript Client
                //new Client
                //{
                //    ClientId = "js",
                //    ClientName = "eShop SPA OpenId Client",
                //    AllowedGrantTypes = GrantTypes.Implicit,
                //    AllowAccessTokensViaBrowser = true,
                //    RedirectUris =           { $"{clientsUrl["Spa"]}/" },
                //    RequireConsent = false,
                //    PostLogoutRedirectUris = { $"{clientsUrl["Spa"]}/" },
                //    AllowedCorsOrigins =     { $"{clientsUrl["Spa"]}" },
                //    AllowedScopes =
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        "orders",
                //        "basket",
                //        "locations",
                //        "marketing"
                //    }
                //},
                //new Client
                //{
                //    ClientId = "xamarin",
                //    ClientName = "eShop Xamarin OpenId Client",
                //    AllowedGrantTypes = GrantTypes.Hybrid,                    
                //    //Used to retrieve the access token on the back channel.
                //    ClientSecrets =
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    RedirectUris = { clientsUrl["Xamarin"] },
                //    RequireConsent = false,
                //    RequirePkce = true,
                //    PostLogoutRedirectUris = { $"{clientsUrl["Xamarin"]}/Account/Redirecting" },
                //    AllowedCorsOrigins = { "http://eshopxamarin" },
                //    AllowedScopes = new List<string>
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        IdentityServerConstants.StandardScopes.OfflineAccess,
                //        "orders",
                //        "basket",
                //        "locations",
                //        "marketing"
                //    },
                //    //Allow requesting refresh tokens for long lived API access
                //    AllowOfflineAccess = true,
                //    AllowAccessTokensViaBrowser = true
                //},
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    //AccessTokenType = AccessTokenType.Reference,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientUri = $"{clientsUrl["Mvc"]}",                             // public uri of the client
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowAccessTokensViaBrowser = false,
                    RequireConsent = false,
                    AllowOfflineAccess = false,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = new List<string>
                    {
                        $"{clientsUrl["Mvc"]}/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{clientsUrl["Mvc"]}/signout-callback-oidc"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        //"orders",
                        //"basket",
                        //"locations",
                        //"marketing"
                    },
                },
                //new Client
                //{
                //    ClientId = "mvctest",
                //    ClientName = "MVC Client Test",
                //    ClientSecrets = new List<Secret>
                //    {
                //        new Secret("secret".Sha256())
                //    },
                //    ClientUri = $"{clientsUrl["Mvc"]}",                             // public uri of the client
                //    AllowedGrantTypes = GrantTypes.Hybrid,
                //    AllowAccessTokensViaBrowser = true,
                //    RequireConsent = false,
                //    AllowOfflineAccess = true,
                //    RedirectUris = new List<string>
                //    {
                //        $"{clientsUrl["Mvc"]}/signin-oidc"
                //    },
                //    PostLogoutRedirectUris = new List<string>
                //    {
                //        $"{clientsUrl["Mvc"]}/signout-callback-oidc"
                //    },
                //    AllowedScopes = new List<string>
                //    {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        IdentityServerConstants.StandardScopes.OfflineAccess,
                //        "orders",
                //        "basket",
                //        "locations",
                //        "marketing"
                //    },
                //},
                //new Client
                //{
                //    ClientId = "locationsswaggerui",
                //    ClientName = "Locations Swagger UI",
                //    AllowedGrantTypes = GrantTypes.Implicit,
                //    AllowAccessTokensViaBrowser = true,

                //    RedirectUris = { $"{clientsUrl["LocationsApi"]}/swagger/o2c.html" },
                //    PostLogoutRedirectUris = { $"{clientsUrl["LocationsApi"]}/swagger/" },

                //    AllowedScopes =
                //    {
                //        "locations"
                //    }
                //},
                //new Client
                //{
                //    ClientId = "marketingswaggerui",
                //    ClientName = "Marketing Swagger UI",
                //    AllowedGrantTypes = GrantTypes.Implicit,
                //    AllowAccessTokensViaBrowser = true,

                //    RedirectUris = { $"{clientsUrl["MarketingApi"]}/swagger/o2c.html" },
                //    PostLogoutRedirectUris = { $"{clientsUrl["MarketingApi"]}/swagger/" },

                //    AllowedScopes =
                //    {
                //        "marketing"
                //    }
                //},
                //new Client
                //{
                //    ClientId = "basketswaggerui",
                //    ClientName = "Basket Swagger UI",
                //    AllowedGrantTypes = GrantTypes.Implicit,
                //    AllowAccessTokensViaBrowser = true,

                //    RedirectUris = { $"{clientsUrl["BasketApi"]}/swagger/o2c.html" },
                //    PostLogoutRedirectUris = { $"{clientsUrl["BasketApi"]}/swagger/" },

                //    AllowedScopes =
                //    {
                //        "basket"
                //    }
                //},
                //new Client
                //{
                //    ClientId = "orderingswaggerui",
                //    ClientName = "Ordering Swagger UI",
                //    AllowedGrantTypes = GrantTypes.Implicit,
                //    AllowAccessTokensViaBrowser = true,

                //    RedirectUris = { $"{clientsUrl["OrderingApi"]}/swagger/o2c.html" },
                //    PostLogoutRedirectUris = { $"{clientsUrl["OrderingApi"]}/swagger/" },

                //    AllowedScopes =
                //    {
                //        "orders"
                //    }
                //}
            };
        }
    }
}

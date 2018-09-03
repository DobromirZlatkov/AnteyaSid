namespace AnteyaSidOnContainers.Services.Identity.API.Configuration
{
    using IdentityServer4;
    using IdentityServer4.Models;
    using System.Collections.Generic;

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
                new ApiResource("catalog", "Catalog API"),
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

//                 {
//   "id": "nova_mobile",
//   "name": "Lykke Mobile Client",
//   "allowed_cors_origins": [],
//   "redirect_uris": [
//     "lykketradingapp://auth/callback"
//   ],
//   "post_logout_redirect_uris": [],
//   "allowed_scopes": [
//     "openid",
//     "profile",
//     "email",
//     "nova_api"
//   ],
//   "access_token_type": "Reference",
//   "allowed_grant_types": [
//     "authorization_code",
//     "client_credentials"
//   ],
//   "allow_access_tokens_via_browser": false,
//   "allow_offline_access": true,
//   "require_client_secret": false,
//   "require_pkce": false,
//   "require_consent": false,
//   "enabled": true
// }
                new Client
                {
                    ClientId = "nova_mobile",
                    ClientName = "Lykke Mobile Client",
                   // ClientUri = "lykketradingapp://",
                    RedirectUris = new List<string>
                    {
                        "lykketradingapp://auth/callback"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "lykketradingapp://auth/callback",
                        "http://nova.lykkecloud.com"
                    },
                    AllowedGrantTypes = new List<string>
                    {
                        "authorization_code",
                        "client_credentials"
                    },
                    AllowAccessTokensViaBrowser = false,
                    AllowOfflineAccess = true,
                    RequireClientSecret = false,
                    Enabled = true,
                    RequirePkce = false,
                    RequireConsent = false,

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    }

                },
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientUri = $"{clientsUrl["Mvc"]}",
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
                        "catalog"
                    },
                },
                new Client
                {
                    ClientId = "catalogswaggerui",
                    ClientName = "Catalog Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientsUrl["CatalogApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientsUrl["CatalogApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "catalog"
                    }
                },
            };
        }
    }
}

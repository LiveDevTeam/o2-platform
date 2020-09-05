using System;
using System.Collections.Generic;
using System.Diagnostics;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace O2.Identity.API
{ public class Config
    {
        //public static Dictionary<string, string> ClientUrls { get; private set; }
        public static Dictionary<string, string> GetUrls(IConfiguration configuration)
        {
            Dictionary<string, string> urls = new Dictionary<string, string>();

            urls.Add("ShopMvc", configuration.GetValue<string>("ShopMvcClient"));

            return urls;

        }
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                 new ApiResource("basket", "Shopping Cart Api"),
                 new ApiResource("orders", "Ordering Api"),
            };
        }



        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
               // new IdentityResources.Email()
            };
        }
        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientUrls)
        {

            return new List<Client>()
            {
                new Client
                {
                    ClientId = "ShopMvc",
                    ClientSecrets = new [] { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    RedirectUris = {$"{clientUrls["ShopMvc"]}/signin-oidc"},
                    PostLogoutRedirectUris = {$"{clientUrls["ShopMvc"]}/signout-callback-oidc"},
                    AllowAccessTokensViaBrowser = false,
                    AllowOfflineAccess = true,
                    RequireConsent = false,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes = new List<string>
                    {

                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                      //  IdentityServerConstants.StandardScopes.Email,
                         "orders",
                        "basket",

                    }

                }
            };
        }

    }
}

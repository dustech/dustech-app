using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Dustech.App.Infrastructure;

namespace Dustech.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            { };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client()
            {
                ClientName = Configs.razorPagesWebClient.ClientName,
                ClientId = Configs.razorPagesWebClient.ClientId,
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris =
                {
                    Configs.razorPagesWebClient.RedirectUri
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },
                ClientSecrets =
                {
                    new Secret(Configs.razorPagesWebClient.HashedClientSecret)
                },
                RequireConsent = true
            }
        };
}
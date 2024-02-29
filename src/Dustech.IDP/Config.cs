using Duende.IdentityServer;
using Duende.IdentityServer.Models;

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
                ClientName = App.Infrastructure.Config.razorPagesWebClient.ClientName,
                ClientId = App.Infrastructure.Config.razorPagesWebClient.ClientId,
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris =
                {
                    App.Infrastructure.Config.razorPagesWebClient.RedirectUri,
                    //App.Infrastructure.Config.razorPagesWebClient.RedirectUri2
                },
                PostLogoutRedirectUris =
                {
                    App.Infrastructure.Config.razorPagesWebClient.SignedOutCallbackPath
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },
                ClientSecrets =
                {
                    new Secret(App.Infrastructure.Config.razorPagesWebClient.HashedClientSecret)
                },
                RequireConsent = true
            }
        };
}
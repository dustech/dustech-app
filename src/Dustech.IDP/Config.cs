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

    public static IEnumerable<Client> Clients(App.Infrastructure.Config.Client razorPagesWebClient) =>
        new []
        {
            new Client()
            {
                ClientName = razorPagesWebClient.ClientName,
                ClientId = razorPagesWebClient.ClientId,
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = razorPagesWebClient.RedirectUris,
                PostLogoutRedirectUris =
                    razorPagesWebClient.SignedOutCallbackPaths,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },
                ClientSecrets =
                {
                    new Secret(razorPagesWebClient.HashedClientSecret)
                },
                RequireConsent = true
            }
        };
}
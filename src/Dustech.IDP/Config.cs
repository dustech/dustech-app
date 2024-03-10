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
            new IdentityResources.Profile(),
            new(CustomScopes.Roles.Name, CustomScopes.Roles.DisplayName, CustomScopes.Roles.UserClaims)
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            { };

    public static IEnumerable<Client> Clients(App.Infrastructure.Config.Client razorPagesWebClient) =>
        new[]
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
                    IdentityServerConstants.StandardScopes.Profile,
                    CustomScopes.Roles.Name
                },
                ClientSecrets =
                {
                    new Secret(razorPagesWebClient.HashedClientSecret)
                },
                RequireConsent = true
            }
        };
}
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Dustech.App.Web.Pages.Classified;

[Authorize(Roles = "Administrator")]
public class IndexModel(ILogger<IndexModel> logger) : LayoutModel("Classified")
{
    public override void OnGet()
    {
        base.OnGet();

#pragma warning disable CA2008
        new TaskFactory().StartNew(LogIdentityInformation);
#pragma warning restore CA2008

    }
    public ILogger<IndexModel> Logger { get; } = logger;

    [SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task")]
    [SuppressMessage("Globalization", "CA1305:Specify IFormatProvider")]
    [SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates")]
    [SuppressMessage("Usage", "CA2254:Template should be a static expression")]
    public async Task LogIdentityInformation()
    {
        // get the saved identity token
        var identityToken = await HttpContext
            .GetTokenAsync(OpenIdConnectParameterNames.IdToken);
        
        //
        // // get the saved access token
        // var accessToken = await HttpContext
        //     .GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
        //
        // // get the refresh token
        // var refreshToken = await HttpContext
        //     .GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
        //
        var userClaimsStringBuilder = new StringBuilder();
        foreach (var claim in User.Claims)
        {
            userClaimsStringBuilder.AppendLine(
                $"Claim type: {claim.Type} - Claim value: {claim.Value}");
        }

        // log token & claims
        Logger.LogInformation($"Identity token & user claims: " +
                              $"\n{identityToken} \n{userClaimsStringBuilder}");
        // Logger.LogInformation($"Access token: " +
        //                       $"\n{accessToken}");
        // Logger.LogInformation($"Refresh token: " +
        //                       $"\n{refreshToken}");
    }
}
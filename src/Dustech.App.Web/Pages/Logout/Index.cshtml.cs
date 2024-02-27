using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dustech.App.Web.Pages.Logout;

[Authorize]
public class IndexModel() : LayoutModel("Logout")
{
    public async Task<IActionResult> OnPostAsync(string action)
    {
        if (action != "logout") return RedirectToPage("/Home/Index");
        
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme).ConfigureAwait(false);
        
        const string logoutUrl = "/Home/Index"; 
        return SignOut(new AuthenticationProperties { RedirectUri = logoutUrl },
            OpenIdConnectDefaults.AuthenticationScheme);

    }
}
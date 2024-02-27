using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dustech.App.Web.Pages;

public class LayoutModel(string title, bool redirectToHome = false) : PageModel
{
    public virtual void OnGet()
    {
        RequestCultureFeature =
            HttpContext.Features.Get<IRequestCultureFeature>();
    }

    public IActionResult OnGetSetCulture(string cultureName)
    {
        var culture = new RequestCulture(cultureName);
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(culture));

        return RedirectToPage();
    }

    public string Title { get; } = title;
    public bool RedirectToHome { get; } = redirectToHome;
    public IRequestCultureFeature? RequestCultureFeature { get; set; }

    public HeadModel HeadModel { get; init; } = new(title, redirectToHome);
}

public record HeadModel(string Title, bool RedirectToHome);
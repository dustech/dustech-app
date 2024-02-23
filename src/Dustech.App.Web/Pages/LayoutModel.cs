using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dustech.App.Web.Pages;

public class LayoutModel : PageModel
{
    public LayoutModel(string title)
    {
        Title = title;
        
    }
    
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

        return RedirectToPage("Index");
    }

    public string Title { get; }
    public IRequestCultureFeature? RequestCultureFeature { get; set; }
}
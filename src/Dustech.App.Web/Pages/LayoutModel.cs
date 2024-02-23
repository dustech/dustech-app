using Microsoft.AspNetCore.Localization;
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

    public string Title { get; }
    public IRequestCultureFeature? RequestCultureFeature { get; set; }
}
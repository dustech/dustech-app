using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dustech.App.Web.Pages;


public class LayoutModel(string title) : PageModel
{
    public string Title { get;} = title;
}


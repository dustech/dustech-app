using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Dustech.App.Web.Pages;

public class IndexModel() : LayoutModel("Home page")
{
    public void OnGet()
    {
        // return new LayoutModel<IndexModel>(this, "Home page");
        //return Page(new LayoutModel<IndexModel>(this, "Home page"));
    }
}

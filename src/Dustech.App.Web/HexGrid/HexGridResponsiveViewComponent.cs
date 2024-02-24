using Microsoft.AspNetCore.Mvc;

namespace Dustech.App.Web.HexGrid;



public class HexGridResponsiveViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(HexGridResponsiveViewComponentModel model)
    {
        return View(model);
    } 
}
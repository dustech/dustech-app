using Microsoft.AspNetCore.Mvc;

namespace Dustech.App.Web.HexGrid;

public class HexViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(HexViewComponentModel model)
    {

        return View(model);
    }
}
using System;
using Microsoft.AspNetCore.Mvc;

namespace Dustech.App.Web.HexGrid;



public class HexGridResponsiveViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(HexGridResponsiveViewComponentModel model)
    {
        return View(model);
    } 
}


public record HexGridResponsiveViewComponentModel
{
    public HexGridResponsiveViewComponentModel(HexGridViewComponentModel hexGridModel)
    {
        ArgumentNullException.ThrowIfNull(hexGridModel);
        HexGridModel = hexGridModel;

    }
    public HexGridViewComponentModel HexGridModel { get; init; }
}
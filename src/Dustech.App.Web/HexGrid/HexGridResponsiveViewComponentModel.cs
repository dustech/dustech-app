using System;

namespace Dustech.App.Web.HexGrid;

public record HexGridResponsiveViewComponentModel
{
    public HexGridResponsiveViewComponentModel(HexGridViewComponentModel hexGridModel)
    {
        ArgumentNullException.ThrowIfNull(hexGridModel);
        HexGridModel = hexGridModel;

    }
    public HexGridViewComponentModel HexGridModel { get; init; }
}
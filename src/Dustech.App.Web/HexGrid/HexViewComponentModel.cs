using System;

namespace Dustech.App.Web.HexGrid;

public record HexViewComponentModel
{
    public static double HexWidth (double height) => height * Math.Cos(HexRad);
    const double HexAngle = 30;
    const double HexRad = HexAngle * (Math.PI / 180);
    public double TopOffSet { get; init; }
    public double LeftOffSet { get; init; }
    public string ColorContext { get; init; } = "primary";
    public double Height { get; init; }

    public double Width => HexWidth(Height);

    public string Content { get; init; } = "";
    public string Icon { get; init; } = "";
    public string Href { get; init; } = "/index";
}
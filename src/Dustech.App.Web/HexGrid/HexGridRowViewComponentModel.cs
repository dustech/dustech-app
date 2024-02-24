using System;
using System.Collections.Generic;
using System.Linq;

namespace Dustech.App.Web.HexGrid;

public record HexGridRowViewComponentModel
{
    public HexGridRowViewComponentModel(
        IEnumerable<HexViewComponentModel> elements)
    {
        var hexViewComponentModels = elements.ToList();
        if (hexViewComponentModels.Count == 0)
        {
            throw new ArgumentException("Elements collection cannot be empty.",
                nameof(elements));
        }

        Elements = hexViewComponentModels;
    }
    public IEnumerable<HexViewComponentModel> Elements { get; init; }
    public int RowNumber { get; init; }
    public double Margin { get; init; }

    public double RowLeftOffSet => (RowNumber % 2 == 0)
        ? 0.0
        : LeftCellOffSet / 2;

    public double RowLeftOffSetInverted => (RowNumber % 2 == 0)
        ? LeftCellOffSet / 2
        : 0.0;
    
    public double LeftCellOffSet => ElementWidth + Margin;
    public double TopCellOffSet => ElementHeight * 0.75 + Margin;
    
    private double ElementWidth => Elements.ElementAt(0).Width;
    private double ElementHeight => Elements.ElementAt(0).Height;
    public bool Inverted { get; init; }
}
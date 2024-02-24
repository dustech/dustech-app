using System;
using System.Collections.Generic;
using System.Linq;

namespace Dustech.App.Web.HexGrid;

public record HexGridViewComponentModel
{
    public HexGridViewComponentModel(
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
    public double CellHeight => HexHeight + Margin;
    public double CellWidth => HexWidth + Margin;
    public int RowCount => Rows.Count;

    public double ContainerHeight =>
        // FirstRow
        CellHeight +
        // Other rows times vertical offset CellHeight 
        (RowCount - 1) * CellHeight * 0.75;

    public double ContainerWidth => CellWidth * MaxRowLength;

    public ICollection<HexGridRowViewComponentModel>
        Rows { get; init; } =
        [];

    public int MaxRowLength { get; init; }
    public double Margin { get; init; }
    public double HexHeight { get; init; }
    private double HexWidth => HexViewComponentModel.HexWidth(HexHeight);
    
    public string ContainerClass { get; init; } = "";

    public bool Inverted { get; init; }
};
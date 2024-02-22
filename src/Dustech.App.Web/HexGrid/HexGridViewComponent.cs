using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Dustech.App.Web.HexGrid;

public class HexGridViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(
        HexGridViewComponentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        
        var rowLength = model.MaxRowLength;
        var hexes = model.Elements
            .Select(h => h with { Height = model.HexHeight }).ToList();

        var margin = model.Margin;
        ICollection<HexGridRowViewComponentModel> rows =
            CutRowsTailRecursive(hexes, 0, rowLength, [], model.Inverted)
                .Select((r, i) => new HexGridRowViewComponentModel(r)
                {
                    RowNumber = i,
                    Margin = margin,
                    Inverted = model.Inverted
                })
                .ToList();
        ;

        model = model with
        {
            Rows = rows
        };

        return View(model);
    }


    private static IEnumerable<IEnumerable<HexViewComponentModel>>
        CutRowsTailRecursive(IReadOnlyCollection<HexViewComponentModel> hexes,
            int i,
            int rowLength,
            ICollection<IEnumerable<HexViewComponentModel>> accumulator,
            bool inverted)
    {
        if (hexes.Count == 0)
        {
            return accumulator;
        }
        var tunedRowLength = inverted switch
        {
            false => i % 2 == 0 ? rowLength : rowLength - 1,
            true => i % 2 != 0 ? rowLength : rowLength - 1,
        };
        
        var currentRow = hexes.Take(tunedRowLength);
        accumulator.Add(currentRow);

        var hsRest = hexes.Skip(tunedRowLength).ToList();
        var index = i + 1;

        return CutRowsTailRecursive(hsRest, index, rowLength, accumulator, inverted);
    }
}

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
    public int RowCount => Rows.Count;

    public double ContainerHeight =>
        // FirstRow
        CellHeight +
        // Other rows times vertical offset CellHeight 
        (RowCount - 1) * CellHeight * 0.75;


    public ICollection<HexGridRowViewComponentModel>
        Rows { get; init; } =
        [];

    public int MaxRowLength { get; init; }
    public double Margin { get; init; }
    public double HexHeight { get; init; }
    public string ContainerClass { get; init; } = "";

    public bool Inverted { get; init; }
};
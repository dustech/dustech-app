using System;
using System.Collections.Generic;
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
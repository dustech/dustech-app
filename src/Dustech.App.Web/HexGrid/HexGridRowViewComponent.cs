using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Dustech.App.Web.HexGrid;

public class HexGridRowViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(HexGridRowViewComponentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        
        var rowModelElements
            = model.Elements.Select((e, index)
                => e with
                {
                    LeftOffSet = (model.Inverted? model.RowLeftOffSetInverted: model.RowLeftOffSet) +
                                 model.LeftCellOffSet * index,
                    TopOffSet = model.TopCellOffSet * (model.RowNumber),
                    Content = $"{e.Content} {model.RowNumber},{index}"
                });

        var rowModel = model with { Elements = rowModelElements };
        return View(rowModel);
    }
}

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
using System;
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
                    Content = $"{e.Content}"
                });

        var rowModel = model with { Elements = rowModelElements };
        return View(rowModel);
    }
}
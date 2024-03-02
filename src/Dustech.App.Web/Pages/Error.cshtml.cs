using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dustech.App.Web.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None,
    NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : LayoutModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    private readonly ILogger<ErrorModel> _logger;

    public ErrorModel(ILogger<ErrorModel> logger):base("Error")
    {
        _logger = logger;
    }

    public override void OnGet()
    {
        base.OnGet();
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dustech.App.Web.Pages.Errors;

public class AccessDeniedModel() : LayoutModel("Access Denied",redirectToHome: false);
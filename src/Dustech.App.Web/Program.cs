using System;
using System.Security.Cryptography.X509Certificates;
using Dustech.App.Infrastructure;
using static Dustech.App.Infrastructure.ConfigurationParser;
using static Dustech.App.Infrastructure.ConfigurationParser.WebAppConfigurationParser;
using Dustech.App.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var supportedCultures = new[] { "en", "it" };
var builder = WebApplication.CreateBuilder(args);

var x509Key = builder.Configuration.GetSection("x509:key").Value;
var x509 = new X509Certificate2("/certs/dustech-io.pfx",x509Key);
using (x509)
{
    


// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Home/Index", "/");
    options.Conventions.AddPageRoute("/Errors/AccessDenied", "/AccessDenied");
}).AddViewLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures)
        .SetDefaultCulture("en");
});

builder.Services.AddLocalization(options =>
    options.ResourcesPath = "Resources"
);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddWebOptimizer(minifyJavaScript: false, minifyCss: false);
}
else
{
    builder.Services.AddWebOptimizer();
}

var idpSection = builder.Configuration.GetSection(nameof(WebAppConfiguration));
var idpConfiguration = parseWebAppConfiguration(idpSection);


builder.Services.AddOpenIdConnectServices(idpConfiguration,x509);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseWebOptimizer();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization();

app.MapRazorPages();

app.Run();

}
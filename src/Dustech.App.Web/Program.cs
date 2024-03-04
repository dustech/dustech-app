using System;
using System.Security.Cryptography.X509Certificates;
using Dustech.App.Infrastructure;
using static Dustech.App.Infrastructure.ConfigurationParser;
using static Dustech.App.Infrastructure.ConfigurationParser.RuntimeConfigurationParser;
using Dustech.App.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Dustech.App.Infrastructure.ConfigurationParser.DataProtectionConfigurationParser;
var supportedCultures = new[] { "en", "it" };
var builder = WebApplication.CreateBuilder(args);


var dataProtectionConfiguration = parseDataProtectionConfiguration(builder.Configuration.GetSection(nameof(DataProtectionConfiguration)));
var webAppConfiguration = parseWebAppConfiguration(builder.Configuration.GetSection(nameof(WebAppConfiguration)));

Console.WriteLine($"X509__Key {dataProtectionConfiguration.X509__Key}");
Console.WriteLine($"X509__FileName {dataProtectionConfiguration.X509__FileName}");
Console.WriteLine($"X509__Path {dataProtectionConfiguration.X509__Path}");
Console.WriteLine($"X509Location {dataProtectionConfiguration.X509Location}");
Console.WriteLine($"DataProtectionPath {dataProtectionConfiguration.DataProtectionPath}");

var x509 = new X509Certificate2(dataProtectionConfiguration.X509Location,dataProtectionConfiguration.X509__Key);
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



builder.Services.AddOpenIdConnectServices(webAppConfiguration,dataProtectionConfiguration,x509);

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
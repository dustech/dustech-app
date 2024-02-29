using System;
using System.Net.Http;
using Dustech.App.Infrastructure;
using Dustech.App.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var supportedCultures = new[] { "en", "it" };
var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddOpenIdConnectServices();

// builder.Services.Configure<ForwardedHeadersOptions>(options =>
// {
//     options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
// });

var app = builder.Build();
//app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseWebOptimizer();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization();

app.MapRazorPages();

app.Run();

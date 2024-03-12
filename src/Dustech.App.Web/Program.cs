using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Dustech.App.DAL;
using Dustech.App.Domain;
using Dustech.App.Infrastructure;
using static Dustech.App.Infrastructure.ConfigurationParser.RuntimeConfigurationParser;
using Dustech.App.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Net.Http.Headers;
using static Dustech.App.Infrastructure.ConfigurationParser.DataProtectionConfigurationParser;


var supportedCultures = new[] { "en", "it" };
var builder = WebApplication.CreateBuilder(args);


var dataProtectionConfiguration =
    parseDataProtectionConfiguration(builder.Configuration.GetSection(nameof(DataProtectionConfiguration)));
var webAppConfiguration = parseWebAppConfiguration(builder.Configuration.GetSection(nameof(WebAppConfiguration)));
var razorPagesWebClient =
    Config.razorPagesWebClient(webAppConfiguration.Authority, webAppConfiguration.WebAppInternalUri);

// Console.WriteLine($"X509__Key {dataProtectionConfiguration.X509__Key}");
// Console.WriteLine($"X509__FileName {dataProtectionConfiguration.X509__FileName}");
// Console.WriteLine($"X509__Path {dataProtectionConfiguration.X509__Path}");
// Console.WriteLine($"X509Location {dataProtectionConfiguration.X509Location}");
// Console.WriteLine($"DataProtectionPath {dataProtectionConfiguration.DataProtectionPath}");
// Console.WriteLine($"Proxied {webAppConfiguration.Proxied}");
// Console.WriteLine($"WebAppInternalUri {webAppConfiguration.WebAppInternalUri}");
// Console.WriteLine($"Authority {webAppConfiguration.Authority}");

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

JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();


builder.Services.AddOpenIdConnectServices(webAppConfiguration, razorPagesWebClient);

var x509 = new X509Certificate2(dataProtectionConfiguration.X509Location, dataProtectionConfiguration.X509__Key);


builder.Services.AddWebappDataProtection(dataProtectionConfiguration, razorPagesWebClient,
    x509);


var pwd = Environment.GetEnvironmentVariable("PSLpwd");
var usr = Environment.GetEnvironmentVariable("PSLusr");
var host = Environment.GetEnvironmentVariable("PSLhost");
var database = Environment.GetEnvironmentVariable("PSLdatabase");


// var usersInMemory = UsersInMemory.toUsers(ExampleUsers.exampleUsers);
var conData = new Common.ConnectionData(password: pwd, username: usr, host: host, database: database);
var usersInPostgres = UsersInDatabase.toUsersInPostgres(conData);

builder.Services.TryAddSingleton<Users.IUser>(usersInPostgres);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseWebOptimizer();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if (ctx.File.Name == "background.mp4")
        {
            ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=1800";
        }
    }
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization();

app.MapRazorPages();

app.Run();

x509.Dispose();

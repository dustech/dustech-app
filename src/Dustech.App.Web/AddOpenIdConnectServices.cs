using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Dustech.App.Infrastructure;
using static Dustech.App.Infrastructure.ConfigurationParser.RuntimeConfigurationParser;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NUglify.Helpers;
using static Dustech.App.Infrastructure.ConfigurationParser.DataProtectionConfigurationParser;
namespace Dustech.App.Web;

public static class OpenIdConnectServicesExtensions
{
    public static IServiceCollection AddOpenIdConnectServices(this IServiceCollection services,
        WebAppConfiguration webAppConfiguration,
        DataProtectionConfiguration dataProtectionConfiguration,
        Config.Client razorPagesWebClient,
        X509Certificate2 x509)
    {
        ArgumentNullException.ThrowIfNull(webAppConfiguration);
        ArgumentNullException.ThrowIfNull(dataProtectionConfiguration);
        ArgumentNullException.ThrowIfNull(razorPagesWebClient);
        ArgumentNullException.ThrowIfNull(x509);
        services.AddAuthentication(options =>
            {
                options.DefaultScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                    OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options => { options.AccessDeniedPath = "/AccessDenied"; })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
                options =>
                {
                    var proxied = webAppConfiguration.Proxied;

                    if (proxied)
                    {
                        options.Events.OnRedirectToIdentityProvider = context => Hijacker.Hijack(webAppConfiguration, context);
                        options.Events.OnRedirectToIdentityProviderForSignOut = context => Hijacker.Hijack(webAppConfiguration, context);;
                    }

                    options.SignInScheme = CookieAuthenticationDefaults
                        .AuthenticationScheme;
                    options.SignedOutCallbackPath = new PathString(razorPagesWebClient.SignOutCallBackPath);
                    options.Authority = razorPagesWebClient.Authority;
                    options.ClientId = razorPagesWebClient.ClientId;
                    options.ClientSecret =
                        razorPagesWebClient.ClientSecret;
                    options.ResponseType =
                        razorPagesWebClient.ResponseType;
                    razorPagesWebClient.Scopes
                        .ForEach(options.Scope.Add);
                    options.CallbackPath =
                        new PathString(razorPagesWebClient.CallBackPath);
                    options.AccessDeniedPath = "/AccessDenied";
                    options.SaveTokens = true;
                    options.RequireHttpsMetadata = false;
                });

            
        services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionConfiguration.DataProtectionPath))
                    .ProtectKeysWithCertificate(x509)
                    .SetApplicationName(razorPagesWebClient.ClientId);
            
            
        return services;
    }
}
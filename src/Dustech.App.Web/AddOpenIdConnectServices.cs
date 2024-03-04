﻿using System;
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
        X509Certificate2 x509)
    {
        ArgumentNullException.ThrowIfNull(webAppConfiguration);
        ArgumentNullException.ThrowIfNull(dataProtectionConfiguration);
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
                        options.Events.OnRedirectToIdentityProvider = Hijacker.Hijack;
                        options.Events.OnRedirectToIdentityProviderForSignOut = Hijacker.Hijack;
                    }

                    options.SignInScheme = CookieAuthenticationDefaults
                        .AuthenticationScheme;
                    options.SignedOutCallbackPath = new PathString(Config.razorPagesWebClient.SignOutCallBackPath);
                    options.Authority = Config.razorPagesWebClient.Authority;
                    options.ClientId = Config.razorPagesWebClient.ClientId;
                    options.ClientSecret =
                        Config.razorPagesWebClient.ClientSecret;
                    options.ResponseType =
                        Config.razorPagesWebClient.ResponseType;
                    Config.razorPagesWebClient.Scopes
                        .ForEach(options.Scope.Add);
                    options.CallbackPath =
                        new PathString(Config.razorPagesWebClient
                            .CallBackPath);
                    options.AccessDeniedPath = "/AccessDenied";
                    options.SaveTokens = true;
                    options.RequireHttpsMetadata = false;
                });

            
        services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionConfiguration.DataProtectionPath))
                    .ProtectKeysWithCertificate(x509)
                    .SetApplicationName(Config.razorPagesWebClient.ClientId);
            
            
        return services;
    }
}
using System;
using Dustech.App.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NUglify.Helpers;

namespace Dustech.App.Web;

public static class OpenIdConnectServicesExtensions
{
    public static IServiceCollection AddOpenIdConnectServices(this IServiceCollection services,
        IdpConfigurationParser.IdpConfiguration idpConfiguration)
    {
        ArgumentNullException.ThrowIfNull(services);
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
                    var proxied = idpConfiguration.Proxied;

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
        
        return services;
    }
}
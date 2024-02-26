using System;
using System.Linq;
using Dustech.App.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NUglify.Helpers;

namespace Dustech.App.Web;

public static class OpenIdConnectServicesExtensions
{
    public static IServiceCollection AddOpenIdConnectServices(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddAuthentication(options =>
            {
                options.DefaultScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                    OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
                options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults
                        .AuthenticationScheme;
                    options.Authority = Configs.razorPagesWebClient.Authority;
                    options.ClientId = Configs.razorPagesWebClient.ClientId;
                    options.ClientSecret =
                        Configs.razorPagesWebClient.ClientSecret;
                    options.ResponseType =
                        Configs.razorPagesWebClient.ResponseType;
                    Configs.razorPagesWebClient.Scopes
                        .ForEach(options.Scope.Add);
                    options.CallbackPath =
                        new PathString(Configs.razorPagesWebClient
                            .CallBackPath);
                    options.SaveTokens = true;
                });

        return services;
    }
}
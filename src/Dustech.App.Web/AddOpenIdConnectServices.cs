using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Dustech.App.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NUglify.Helpers;

namespace Dustech.App.Web;

public class CustomHttpMessageHandler : HttpClientHandler
{
    private string _authority { get; set; }
    private string _internalAuthority { get; set; }

    public CustomHttpMessageHandler(string authority, string internalAuthority)
    {
        _authority = authority;
        _internalAuthority = internalAuthority;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.RequestUri = new Uri(request.RequestUri.OriginalString.Replace(_authority, _internalAuthority));
        return base.SendAsync(request, cancellationToken);
    }
}

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
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.AccessDeniedPath = "/AccessDenied";
                })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
                options =>
                {
                    //options.MetadataAddress = "http://localhost:5001/.well-known/openid-configuration";
                    options.BackchannelHttpHandler = new CustomHttpMessageHandler(Config.razorPagesWebClient.Authority,
                        Config.razorPagesWebClient.InternalDuende);


                     
                    options.Events.OnRedirectToIdentityProvider = async context =>
                    {
                        if (context.ProtocolMessage.RedirectUri.StartsWith("http://"))
                        {
                            context.ProtocolMessage.RedirectUri = context.ProtocolMessage.RedirectUri.Replace("http://", "https://");
                        }
                        await Task.FromResult(0);
                    };

                    options.SignInScheme = CookieAuthenticationDefaults
                        .AuthenticationScheme;
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
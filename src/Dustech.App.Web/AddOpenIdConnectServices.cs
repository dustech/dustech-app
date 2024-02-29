using System;
using System.Threading.Tasks;
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
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                options => { options.AccessDeniedPath = "/AccessDenied"; })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
                options =>
                {
                    
                    options.Events.OnRedirectToIdentityProviderForSignOut = async context =>
                    {

#pragma warning disable CA1310
                        if (context.ProtocolMessage?.PostLogoutRedirectUri.StartsWith("https://localhost:5001") != null)
#pragma warning restore CA1310
                        {
                            Console.WriteLine("dentro la if PostLogoutRedirectUri");
                            Console.WriteLine($"context.ProtocolMessage.PostLogoutRedirectUri {context.ProtocolMessage.PostLogoutRedirectUri}");
#pragma warning disable CA1307
                            context.ProtocolMessage.PostLogoutRedirectUri = context.ProtocolMessage.PostLogoutRedirectUri.Replace("https://localhost:5001", "https://auth.dustech.io");
#pragma warning restore CA1307
                            
                            Console.WriteLine("dopo la sub PostLogoutRedirectUri");
                            Console.WriteLine($"context.ProtocolMessage.PostLogoutRedirectUri {context.ProtocolMessage.PostLogoutRedirectUri}");
                        }
                        
                         
#pragma warning disable CA1310
                        if (context.ProtocolMessage?.IssuerAddress.StartsWith("https://localhost:5001") != null)
#pragma warning restore CA1310
                        {
                            Console.WriteLine("dentro la if IssuerAddress");
                            Console.WriteLine($"context.ProtocolMessage.RedirectUri {context.ProtocolMessage.IssuerAddress}");
#pragma warning disable CA1307
                            context.ProtocolMessage.IssuerAddress = context.ProtocolMessage.IssuerAddress.Replace("https://localhost:5001", "https://auth.dustech.io");
#pragma warning restore CA1307
                            
                            Console.WriteLine("dopo la sub IssuerAddress");
                            Console.WriteLine($"context.ProtocolMessage.RedirectUri {context.ProtocolMessage.IssuerAddress}");
                        }
                        
                        await Task.FromResult(0).ConfigureAwait(false);
                    };
                    
                    options.Events.OnRedirectToIdentityProvider = async context =>
                    {
                        Console.WriteLine(context.ProtocolMessage);
                        Console.WriteLine("dentro OnRedirectToIdentityProvider");
                        Console.WriteLine($"context.ProtocolMessage.RedirectUri {context.ProtocolMessage.RedirectUri}");
#pragma warning disable CA1310
                        if (context.ProtocolMessage.RedirectUri.StartsWith("https://localhost:5001"))
#pragma warning restore CA1310
                        {
                            Console.WriteLine("dentro la if");
                            Console.WriteLine($"context.ProtocolMessage.RedirectUri {context.ProtocolMessage.RedirectUri}");
#pragma warning disable CA1307
                            context.ProtocolMessage.RedirectUri = context.ProtocolMessage.RedirectUri.Replace("https://localhost:5001", "https://auth.dustech.io");
#pragma warning restore CA1307
                            
                            Console.WriteLine("dopo la sub");
                            Console.WriteLine($"context.ProtocolMessage.RedirectUri {context.ProtocolMessage.RedirectUri}");
                        }
                        
#pragma warning disable CA1310
                        if (context.ProtocolMessage.IssuerAddress.StartsWith("https://localhost:5001"))
#pragma warning restore CA1310
                        {
                            Console.WriteLine("dentro la if IssuerAddress");
                            Console.WriteLine($"context.ProtocolMessage.RedirectUri {context.ProtocolMessage.IssuerAddress}");
#pragma warning disable CA1307
                            context.ProtocolMessage.IssuerAddress = context.ProtocolMessage.IssuerAddress.Replace("https://localhost:5001", "https://auth.dustech.io");
#pragma warning restore CA1307
                            
                            Console.WriteLine("dopo la sub IssuerAddress");
                            Console.WriteLine($"context.ProtocolMessage.RedirectUri {context.ProtocolMessage.IssuerAddress}");
                        }
                        
                     
                        
                        await Task.FromResult(0).ConfigureAwait(false);
                    };
                    
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
                });
        
        return services;
    }
}
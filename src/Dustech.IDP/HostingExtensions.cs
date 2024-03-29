using System.Security.Cryptography.X509Certificates;
using Dustech.IDP.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;
using static Dustech.App.Infrastructure.ConfigurationParser.DataProtectionConfigurationParser;
using static Dustech.App.Infrastructure.ConfigurationParser.RuntimeConfigurationParser;

namespace Dustech.IDP;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder,
        IdpConfiguration idpConfiguration,
        DataProtectionConfiguration dataProtectionConfiguration,
        App.Infrastructure.Config.Client razorPagesWebClient,
        X509Certificate2 x509)
    {
        ArgumentNullException.ThrowIfNull(idpConfiguration);
        ArgumentNullException.ThrowIfNull(dataProtectionConfiguration);
        ArgumentNullException.ThrowIfNull(razorPagesWebClient);
        ArgumentNullException.ThrowIfNull(x509);
        // uncomment if you want to add a UI
        builder.Services.AddRazorPages();

        builder.Services.AddIdentityServer(options =>
            {
                options.EmitStaticAudienceClaim = true;
                options.KeyManagement.Enabled = false;
                if (idpConfiguration.Proxied)
                {
                    options.IssuerUri = idpConfiguration.Authority;
                }
            })
            .AddSigningCredential(x509)
            .AddProfileService<LocalUserProfileService>()
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients(razorPagesWebClient));
            //.AddTestUsers(TestUsers.Users);

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });


        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionConfiguration.DataProtectionPath))
            .ProtectKeysWithCertificate(x509)
            .SetApplicationName(razorPagesWebClient.ClientId);


        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseForwardedHeaders();
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict }); 
        }

        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();
        
        // uncomment if you want to add a UI
        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
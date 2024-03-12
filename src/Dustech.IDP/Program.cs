using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Dustech.IDP;
using Dustech.IDP.Services;
using Serilog;
using static Dustech.App.Infrastructure.ConfigurationParser.DataProtectionConfigurationParser;
using static Dustech.App.Infrastructure.ConfigurationParser.RuntimeConfigurationParser;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    var dataProtectionConfiguration =
        parseDataProtectionConfiguration(builder.Configuration.GetSection(nameof(DataProtectionConfiguration)));
    var idpConfiguration = parseIdpConfiguration(builder.Configuration.GetSection(nameof(IdpConfiguration)));
    var razorPagesWebClient =
        Dustech.App.Infrastructure.Config.razorPagesWebClient(idpConfiguration.Authority,
            idpConfiguration.WebAppInternalUri);

    // Console.WriteLine($"X509__Key {dataProtectionConfiguration.X509__Key}");
    // Console.WriteLine($"X509__FileName {dataProtectionConfiguration.X509__FileName}");
    // Console.WriteLine($"X509__Path {dataProtectionConfiguration.X509__Path}");
    // Console.WriteLine($"X509Location {dataProtectionConfiguration.X509Location}");
    // Console.WriteLine($"DataProtectionPath {dataProtectionConfiguration.DataProtectionPath}");
    // Console.WriteLine($"Proxied {idpConfiguration.Proxied}");
    // Console.WriteLine($"WebAppInternalUri {idpConfiguration.WebAppInternalUri}");
    // Console.WriteLine($"Authority {idpConfiguration.Authority}");
    
    builder.Services.AddScoped<ILocalUserService, LocalUserService>();
    
    var x509 = new X509Certificate2(dataProtectionConfiguration.X509Location, dataProtectionConfiguration.X509__Key);
   
    
    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo
        .Console(
            outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));

    var app = builder
        .ConfigureServices(idpConfiguration, dataProtectionConfiguration, razorPagesWebClient, x509)
        .ConfigurePipeline();

    app.Run();
    x509?.Dispose();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
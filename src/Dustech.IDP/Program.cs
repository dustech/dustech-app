using System.Security.Cryptography.X509Certificates;
using Dustech.IDP;
using Serilog;
using static Dustech.App.Infrastructure.ConfigurationParser.DataProtectionConfigurationParser;
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    
    
    var builder = WebApplication.CreateBuilder(args);
    var dataProtectionConfiguration = parseDataProtectionConfiguration(builder.Configuration.GetSection(nameof(DataProtectionConfiguration)));
    
    Console.WriteLine($"X509__Key {dataProtectionConfiguration.X509__Key}");
    Console.WriteLine($"X509__FileName {dataProtectionConfiguration.X509__FileName}");
    Console.WriteLine($"X509__Path {dataProtectionConfiguration.X509__Path}");
    Console.WriteLine($"X509Location {dataProtectionConfiguration.X509Location}");
    Console.WriteLine($"DataProtectionPath {dataProtectionConfiguration.DataProtectionPath}");
    
     var x509 = new X509Certificate2(dataProtectionConfiguration.X509Location,dataProtectionConfiguration.X509__Key);
     using (x509)
     {
    
         builder.Host.UseSerilog((ctx, lc) => lc
             .WriteTo
             .Console(
                 outputTemplate:
                 "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
             .Enrich.FromLogContext()
             .ReadFrom.Configuration(ctx.Configuration));
    
         var app = builder
             .ConfigureServices(x509,dataProtectionConfiguration)
             .ConfigurePipeline();
    
         app.Run();
    }
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
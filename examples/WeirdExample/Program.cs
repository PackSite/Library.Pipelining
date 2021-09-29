namespace WeirdExample
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Configuration;
    using Serilog;
    using WeirdExample.Pipelines.Initializers;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            ConfigurationBuilder builder = new();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .CreateLogger();

            Log.Logger.Information("Application building");

            try
            {
                IHostBuilder hostBuidler = Host.CreateDefaultBuilder()
                    .ConfigureServices((context, services) =>
                    {
                        services.AddOptions();

                        services.AddPipelining(builder =>
                        {
                            builder.Services.Configure<PipeliningConfiguration>(context.Configuration.GetSection("Pipelining"));

                            builder.AddConfiguration()
                                   .AddInitializer<SampleAppPipelinesInitializer>();
                        });

                        services.AddHostedService<SampleAppHostedService>();
                    })
                    .UseSerilog();

                await hostBuidler.RunConsoleAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.Warning("Application closed.");
                Log.CloseAndFlush();
            }
        }

        private static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}

﻿namespace SampleApp
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Configuration;
    using SampleApp.Pipelines.Initializers;
    using Serilog;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            ConfigurationBuilder builder = new();
            BuildConfig(builder, args);

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

                        services.AddPipelining()
                                .Configure<PipeliningConfiguration>(context.Configuration.GetSection("Pipelining"))
                                .AddPipelineInitializer<SampleAppPipelinesInitializer>();

                        services.AddSingleton<IHostedService, SampleAppHostedService>();
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

        private static void BuildConfig(IConfigurationBuilder builder, string[] args)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets(typeof(Program).Assembly, optional: true, reloadOnChange: true)
                .AddCommandLine(args);
        }
    }
}
[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
namespace AppConfigurationExample
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Configuration;

    public class Program
    {
        /*
         * This example demonstrates pipelines configuration using IOptions/IConfiguration.
         *
         * `builder.AddConfiguration();` registers a hosted service that registers/updates pipelines in IPipelineCollection
         * on startup and after configuration update.
         *
         * However, it's recommended to use `builder.AddInitializer()` over `builder.AddConfiguration();` for most scenarios
         * to register all pipelines.
         *
         */

        public static async Task Main(string[] args)
        {
            IHostBuilder hostBuidler = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddPipelining(builder =>
                    {
                        builder.Services.Configure<PipeliningConfiguration>(context.Configuration.GetSection("Pipelining"));
                        builder.AddConfiguration();
                    });

                    services.AddHostedService<DemoHostedService>();
                });

            await hostBuidler.RunConsoleAsync();
        }
    }
}

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

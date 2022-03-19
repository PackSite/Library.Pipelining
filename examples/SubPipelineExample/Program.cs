[assembly: System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
namespace SubPipelineExample
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Pipelining;
    using SubPipelineExample.Extensions;
    using SubPipelineExample.Pipelines;

    public class Program
    {
        /*
         * This example demonstrates the usage of sub-pipelining.
         */

        public static async Task Main(string[] args)
        {
            IHostBuilder hostBuidler = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddPipelining(builder =>
                    {
                        builder.AddInitializer(pipelines =>
                        {
                            _ = PipelineBuilder.Create<DemoArgs>()
                                .Description("Simple pipeline.")
                                .Lifetime(InvokablePipelineLifetime.Singleton)
                                .Add<Step1>()
                                .Add<Step2>()
                                .Add<Step3>()
                                .Build()
                                .TryAddTo(pipelines).NullifyFalse() ?? throw new ApplicationException();

                            _ = PipelineBuilder.Create<DemoArgs>()
                                .Name("dynamic-sub-pipeline-demo")
                                .Description("Simple pipeline that is used as a sub-pipeline.")
                                .Lifetime(InvokablePipelineLifetime.Transient)
                                .Add<SubpipelineStep1>()
                                .Add<SubpipelineStep2>()
                                .Build()
                                .TryAddTo(pipelines).NullifyFalse() ?? throw new ApplicationException();
                        });
                    });

                    services.AddHostedService<DemoHostedService>();
                });

            await hostBuidler.RunConsoleAsync();
        }
    }
}

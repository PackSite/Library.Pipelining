namespace PipelineInitializerExample
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Pipelining;
    using PipelineInitializerExample.Extensions;
    using PipelineInitializerExample.Initializers;
    using PipelineInitializerExample.Steps;

    public class Program
    {
        /*
         * This example demonstrates pipelines configuration using `IPipelineInitializer`.
         */

        public static async Task Main(string[] args)
        {
            IHostBuilder hostBuidler = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddPipelining(builder =>
                    {
                        builder
                            .AddInitializer<SampleInitializer>()
                            .AddInitializer(pipelines =>
                            {
                                _ = PipelineBuilder.Create<TextProcessingArgs>()
                                    .Description("Text processing pipeline.")
                                    .Lifetime(InvokablePipelineLifetime.Scoped)
                                    .AddStep<ToUpperTransformStep>()
                                    .AddStep<TrimTransformStep>()
                                    .AddStep(new SurroundWithSquareBracketsTransformStep())
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

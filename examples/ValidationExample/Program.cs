namespace ValidationExample
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Validation;
    using PackSite.Library.Pipelining.Validation.Validators;
    using ValidationExample.Extensions;
    using ValidationExample.Steps;

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
                            .AddInitializer(pipelines =>
                            {
                                _ = PipelineBuilder.Create<TextProcessingArgs>()
                                    .Name("text-processing-pipeline")
                                    .Description("Text processing pipeline.")
                                    .Lifetime(InvokablePipelineLifetime.Scoped)
                                    .Add<ExceptionHandlingStep>()
                                    .Add<ToUpperTransformStep>()
                                    .Add<TrimTransformStep>()
                                    .Add(new SurroundWithSquareBracketsTransformStep())
                                    .Build()
                                    .TryAddTo(pipelines).NullifyFalse() ?? throw new ApplicationException();
                            })
                            .AddValidator(context =>
                            {
                                context.ContainsPipeline("text-processing-pipeline")
                                    .SuchThat(pipeline =>
                                    {
                                        //pipeline.HaveSteps();

                                        //pipeline.NotHaveTransientLifetime();
                                        //TODO: add StartWith(Type step), EndWith(Type step),
                                        //HaveStep(Type step).Before(Type step)/After(Type step)/At(int index)/NotAt/AsFirst()/AsLast()/NotAsFirst/NotAsLast/AsMiddle/NotAsMiddle/NotBefore/NotAfter
                                    });

                                context.ContainsPipeline<TextProcessingArgs>();
                                context.ContainsPipeline(typeof(TextProcessingArgs));
                                context.ContainsPipeline(context.Pipelines.Get("text-processing-pipeline"));

                                context.NotContainsPipeline("invalid");
                                context.NotContainsPipeline<Program>();
                                context.NotContainsPipeline(typeof(Program));
                                context.NotContainsPipeline(PipelineBuilder.Create<Program>().Build());

                                context.NotBeEmpty();
                                context.HaveCountEqualTo(1);
                                context.HaveCountNotEqualTo(2);
                                context.HaveCountLessThan(2);
                                context.HaveCountLessThanOrEqualTo(1);
                                context.HaveCountGreaterThan(0);
                                context.HaveCountGreaterThanOrEqualTo(1);
                            });
                    });

                    services.AddHostedService<DemoHostedService>();
                });

            await hostBuidler.RunConsoleAsync();
        }
    }
}

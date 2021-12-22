namespace InvocationPerformanceBenchmark.BenchmarksDefinitions
{
    using System;
    using System.Threading.Tasks;
    using InvocationPerformanceBenchmark.Extensions;
    using InvocationPerformanceBenchmark.Steps;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Pipelining;

    public sealed class Steps100InvocationPerformanceBenchmark : IBenchmark
    {
        private IHost? HostInstance { get; set; }
        private AsyncServiceScope? Scope { get; set; }
        private IInvokablePipelineFactory? InvokablePipelineFactory { get; set; }

        public Steps100InvocationPerformanceBenchmark()
        {

        }

        public async Task SetupAsync()
        {
            HostInstance = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddPipelining(builder =>
                    {
                        builder
                            .AddInitializer(pipelines =>
                            {
                                _ = PipelineBuilder.Create<ProcessingArgs>()
                                    .Description("Text processing pipeline.")
                                    // 10
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    // 20
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    // 30
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    // 40
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    // 50
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    // 60
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    // 70
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    // 80
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    // 90
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    // 100
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()

                                    .Build()
                                    .TryAddTo(pipelines).NullifyFalse() ?? throw new ApplicationException();
                            });
                    });
                })
                .Build();

            await HostInstance.Services.FakeHostStartAsync();
            Scope = HostInstance.Services.CreateAsyncScope();
            InvokablePipelineFactory = Scope!.Value.ServiceProvider.GetRequiredService<IInvokablePipelineFactory>();
        }

        public async Task BenchmarkAsync()
        {
            IInvokablePipeline<ProcessingArgs> invokablePipeline = InvokablePipelineFactory!.GetRequiredPipeline<ProcessingArgs>();

            for (int m = 0; m < 100; m++)
            {
                await invokablePipeline.InvokeAsync(new ProcessingArgs());
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (HostInstance is not null)
            {
                InvokablePipelineFactory = null;

                if (Scope is not null)
                {
                    await Scope.Value.DisposeAsync();
                }

                await HostInstance.Services.FakeHostStopAsync();
                HostInstance.Dispose();
                HostInstance = null;
            }
        }
    }
}

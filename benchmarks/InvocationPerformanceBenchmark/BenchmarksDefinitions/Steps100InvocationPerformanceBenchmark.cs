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
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    // 20
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    // 30
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    // 40
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    // 50
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    // 60
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    // 70
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    // 80
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    // 90
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    // 100
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()
                                    .Add<NopStep>()

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

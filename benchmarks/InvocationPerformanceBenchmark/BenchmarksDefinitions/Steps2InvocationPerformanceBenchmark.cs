namespace InvocationPerformanceBenchmark.BenchmarksDefinitions
{
    using System;
    using System.Threading.Tasks;
    using InvocationPerformanceBenchmark.Extensions;
    using InvocationPerformanceBenchmark.Steps;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using PackSite.Library.Pipelining;

    public sealed class Steps2InvocationPerformanceBenchmark : IBenchmark
    {
        private IHost? HostInstance { get; set; }
        private IServiceScope? Scope { get; set; }
        private IInvokablePipelineFactory? InvokablePipelineFactory { get; set; }

        public Steps2InvocationPerformanceBenchmark()
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
                                    .AddStep<NopStep>()
                                    .AddStep<NopStep>()
                                    .Build()
                                    .TryAddTo(pipelines).NullifyFalse() ?? throw new ApplicationException();
                            });
                    });
                })
                .Build();

            await HostInstance.Services.FakeHostStartAsync();
            Scope = HostInstance.Services.CreateScope();
            InvokablePipelineFactory = Scope.ServiceProvider.GetRequiredService<IInvokablePipelineFactory>();
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
                Scope?.Dispose();
                await HostInstance.Services.FakeHostStopAsync();
                HostInstance.Dispose();
                HostInstance = null;
            }
        }
    }
}

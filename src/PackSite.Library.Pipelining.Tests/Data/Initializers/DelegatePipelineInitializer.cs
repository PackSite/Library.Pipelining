namespace PackSite.Library.Pipelining.Tests.Data.Initializers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining.Tests.Data.Args;
    using PackSite.Library.Pipelining.Tests.Data.Steps;

    public static class DelegatePipelineInitializer
    {
        public static PipelineName[] Names { get; } = new PipelineName[] { "demo0" };

        public static Action<IPipelineCollection> Simple { get; } = pipelines =>
        {
            _ = PipelineBuilder.Create<SampleArgs>()
                .Name(Names[0])
                .AddStep<StepWithArgs1>()
                .AddStep<StepWithArgs2>()
                .AddStep(new StepWithArgs3())
                .AddStep<GenericStep>()
                .Build().TryAddTo(pipelines);
        };

        public static Func<IServiceProvider, IPipelineCollection, CancellationToken, ValueTask> Complex { get; } = (services, pipelines, cancellationToken) =>
        {
            _ = PipelineBuilder.Create<SampleArgs>()
                .Name(Names[0])
                .AddStep<StepWithArgs1>()
                .AddStep<StepWithArgs2>()
                .AddStep(new StepWithArgs3())
                .AddStep<GenericStep>()
                .Build().TryAddTo(pipelines);

            return default;
        };
    }
}

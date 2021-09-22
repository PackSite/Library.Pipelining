namespace PackSite.Library.Pipelining.Tests.Data
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
                .Step<StepWithArgs1>()
                .Step<StepWithArgs2>()
                .Step(new StepWithArgs3())
                .Step<GenericStep>()
                .Build().TryAddTo(pipelines);
        };

        public static Func<IServiceProvider, IPipelineCollection, CancellationToken, ValueTask> Complex { get; } = (services, pipelines, cancellationToken) =>
        {
            _ = PipelineBuilder.Create<SampleArgs>()
                .Name(Names[0])
                .Step<StepWithArgs1>()
                .Step<StepWithArgs2>()
                .Step(new StepWithArgs3())
                .Step<GenericStep>()
                .Build().TryAddTo(pipelines);

            return default;
        };
    }
}

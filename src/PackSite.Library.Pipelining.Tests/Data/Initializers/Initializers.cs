namespace PackSite.Library.Pipelining.Tests.Data.Initializers
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining.Tests.Data.Args;
    using PackSite.Library.Pipelining.Tests.Data.Steps;

    public sealed class SamplePipelineInitializer : IPipelineInitializer
    {
        public static PipelineName[] Names { get; } = new PipelineName[] { "demo0" };

        public SamplePipelineInitializer()
        {

        }

        public ValueTask RegisterAsync(IPipelineCollection pipelines, CancellationToken cancellationToken)
        {
            _ = PipelineBuilder.Create<SampleArgs>()
                .Name(Names[0])
                .AddStep<StepWithArgs1>()
                .AddStep<StepWithArgs2>()
                .AddStep(new StepWithArgs3())
                .AddStep<GenericStep>()
                .Build().TryAddTo(pipelines);

            return default;
        }
    }
}

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
                .Step<StepWithArgs1>()
                .Step<StepWithArgs2>()
                .Step(new StepWithArgs3())
                .Step<GenericStep>()
                .Build().TryAddTo(pipelines);

            return default;
        }
    }
}

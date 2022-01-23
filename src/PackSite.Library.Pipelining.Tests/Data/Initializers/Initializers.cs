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
                .Add<StepWithArgs1>()
                .Add<StepWithArgs2>()
                .Add(new StepWithArgs3())
                .Add<GenericStep>()
                .Build().TryAddTo(pipelines);

            return default;
        }
    }
}

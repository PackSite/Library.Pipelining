namespace PackSite.Library.Pipelining.Tests.Data
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining.Tests.Data.Contexts;
    using PackSite.Library.Pipelining.Tests.Data.Steps;

    public sealed class SamplePipelineInitializers : IPipelineInitializer
    {
        public static PipelineName[] Names { get; } = new PipelineName[] { "demo0" };

        public SamplePipelineInitializers()
        {

        }

        public ValueTask RegisterAsync(IPipelineCollection pipelines, CancellationToken cancellationToken)
        {
            _ = PipelineBuilder.Create<SampleContext>()
                .Name(Names[0])
                .Add<StepWithContext1>()
                .Add<StepWithContext2>()
                .Add(new StepWithContext3())
                .Add<GenericStep>()
                .Build().TryAddTo(pipelines);

            return default;
        }
    }
}

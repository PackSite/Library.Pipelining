namespace SampleApp.Pipelines.Initializers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using SampleApp.Extensions;
    using SampleApp.Pipelines;
    using SampleApp.Pipelines.DemoData;

    public class SampleAppPipelinesInitializer : IPipelineInitializer
    {
        public ValueTask RegisterAsync(IPipelineCollection pipelines, CancellationToken cancellationToken)
        {
            _ = PipelineBuilder.Create<DemoDataContext>()
                .Description("Demo data pipeline.")
                .Lifetime(InvokablePipelineLifetime.Transient)
                .Add<ExceptionLoggingStep>()
                .Add<DemoDataStep1>()
                .Add<DemoDataStep2>()
                .Build()
                .TryAddTo(pipelines).NullifyFalse() ?? throw new ApplicationException();

            return default;
        }
    }
}

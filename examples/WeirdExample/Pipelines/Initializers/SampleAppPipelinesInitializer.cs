namespace WeirdExample.Pipelines.Initializers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using WeirdExample.Extensions;
    using WeirdExample.Pipelines.DemoData;
    using WeirdExample.Pipelines.Simple;

    public class SampleAppPipelinesInitializer : IPipelineInitializer
    {
        public ValueTask RegisterAsync(IPipelineCollection pipelines, CancellationToken cancellationToken)
        {
            _ = PipelineBuilder.Create<DemoDataArgs>()
                .Description("Demo data pipeline.")
                .Lifetime(InvokablePipelineLifetime.Singleton)
                //.StepInterceptor<SampleStepInterceptor>()
                .Step<ExceptionLoggingStep>()
                .Step<DemoDataStep1>()
                .Step<DemoDataStep2>()
                .Build()
                .TryAddTo(pipelines).NullifyFalse() ?? throw new ApplicationException();

            _ = PipelineBuilder.Create<SimpleArgs>()
                .Description("Simple pipeline.")
                .Lifetime(InvokablePipelineLifetime.Singleton)
                .Step<SimpleStep1>()
                .Step<SimpleStep2>()
                .Step<SimpleStep3>()
                .Build()
                .TryAddTo(pipelines).NullifyFalse() ?? throw new ApplicationException();

            _ = PipelineBuilder.Create<SimpleArgs>()
                .Name("dynamic-subpipeline-demo")
                .Description("Simple pipeline that is used as a subpipeline.")
                .Lifetime(InvokablePipelineLifetime.Transient)
                .Step<SubpipelineStep1>()
                .Step<SubpipelineStep2>()
                .Build()
                .TryAddTo(pipelines).NullifyFalse() ?? throw new ApplicationException();

            return default;
        }
    }
}

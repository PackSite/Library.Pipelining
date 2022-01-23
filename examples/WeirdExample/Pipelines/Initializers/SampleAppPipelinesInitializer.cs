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
                .Add<ExceptionLoggingStep>()
                .Add<DemoDataStep1>()
                .Add<DemoDataStep2>()
                .Build()
                .TryAddTo(pipelines).NullifyFalse() ?? throw new ApplicationException();

            _ = PipelineBuilder.Create<SimpleArgs>()
                .Description("Simple pipeline.")
                .Lifetime(InvokablePipelineLifetime.Singleton)
                .Add<SimpleStep1>()
                .Add<SimpleStep2>()
                .Add<SimpleStep3>()
                .Build()
                .TryAddTo(pipelines).NullifyFalse() ?? throw new ApplicationException();

            _ = PipelineBuilder.Create<SimpleArgs>()
                .Name("dynamic-subpipeline-demo")
                .Description("Simple pipeline that is used as a subpipeline.")
                .Lifetime(InvokablePipelineLifetime.Transient)
                .Add<SubpipelineStep1>()
                .Add<SubpipelineStep2>()
                .Build()
                .TryAddTo(pipelines).NullifyFalse() ?? throw new ApplicationException();

            return default;
        }
    }
}

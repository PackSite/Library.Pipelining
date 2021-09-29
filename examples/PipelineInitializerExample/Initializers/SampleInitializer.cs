namespace PipelineInitializerExample.Initializers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PipelineInitializerExample.Extensions;
    using PipelineInitializerExample.Steps;

    public class SampleInitializer : IPipelineInitializer
    {
        public ValueTask RegisterAsync(IPipelineCollection pipelines, CancellationToken cancellationToken)
        {
            _ = PipelineBuilder.Create<TextProcessingArgs>()
                .Name("text-processsing-pipeline")
                .Description("Text processing pipeline.")
                .Lifetime(InvokablePipelineLifetime.Scoped)
                .Step<ExceptionHandlingStep>()
                .Step<ToUpperTransformStep>()
                .Step<TrimTransformStep>()
                .Step(new SurroundWithSquareBracketsTransformStep())
                .Build()
                .TryAddTo(pipelines).NullifyFalse() ?? throw new ApplicationException();

            return default;
        }
    }
}

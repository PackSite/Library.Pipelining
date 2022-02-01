namespace PackSite.Library.Pipelining.Validation.Rules.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Validation.Validators;

    /// <summary>
    /// Validator that checks whether collection does not contain a pipeline with specific name in it.
    /// </summary>
    public class NotContainsPipelineWithNameRule : CollectionRule
    {
        /// <summary>
        /// Error message used when pipeline was not found.
        ///
        /// Supported placeholders:
        ///  Context,
        ///  PipelineName,
        ///  UnexpectedPipeline
        /// </summary>
        public string ErrorMessage { get; } = "Pipelines collection expected not to contain a pipeline with name '{PipelineName}'.";

        /// <summary>
        /// Pipeline name to check.
        /// </summary>
        protected PipelineName PipelineName { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="NotContainsPipelineWithNameRule"/>.
        /// </summary>
        public NotContainsPipelineWithNameRule(PipelineName pipelineName) :
            base()
        {
            PipelineName = pipelineName ?? throw new ArgumentNullException(nameof(pipelineName));
        }

        /// <inheritdoc/>
        public override ValueTask ValidateAsync(RuleContext<IPipelineCollection> context, CancellationToken cancellationToken = default)
        {
            if (context.Pipelines.GetOrDefault(PipelineName) is IPipeline unexpectedPipeline)
            {
                AddError(context,
                         ErrorMessage,
                         new Dictionary<string, object?>
                         {
                             ["Context"] = context,
                             ["PipelineName"] = PipelineName,
                             ["UnexpectedPipeline"] = unexpectedPipeline,
                         });
            }

            return default;
        }
    }
}

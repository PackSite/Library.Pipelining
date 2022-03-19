namespace PackSite.Library.Pipelining.Validation.Rules.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining.Validation.Validators;

    /// <summary>
    /// Validator that checks whether collection contains a pipeline with specific name in it.
    /// </summary>
    public class ContainsPipelineWithNameRule : CollectionRule
    {
        /// <summary>
        /// Error message used when pipeline was not found.
        ///
        /// Supported placeholders:
        ///  Context,
        ///  PipelineName
        /// </summary>
        public string ErrorMessage { get; } = "Pipelines collection expected to contain a pipeline with name '{PipelineName}'.";

        /// <summary>
        /// Pipeline name to check.
        /// </summary>
        protected PipelineName PipelineName { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CollectionRule"/>.
        /// </summary>
        public ContainsPipelineWithNameRule(PipelineName pipelineName) :
            base()
        {
            PipelineName = pipelineName ?? throw new ArgumentNullException(nameof(pipelineName));
        }

        /// <inheritdoc/>
        public override ValueTask ValidateAsync(RuleContext<IPipelineCollection> context, CancellationToken cancellationToken = default)
        {
            if (context.Pipelines.GetOrDefault(PipelineName) is null)
            {
                AddError(context,
                         ErrorMessage,
                         new Dictionary<string, object?>
                         {
                             ["Context"] = context,
                             ["PipelineName"] = PipelineName,
                         });
            }

            return default;
        }
    }
}

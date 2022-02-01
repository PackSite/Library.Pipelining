namespace PackSite.Library.Pipelining.Validation.Rules.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Validation.Validators;

    /// <summary>
    /// Validator that checks whether collection contains a specific pipeline instance in it.
    /// </summary>
    public class ContainsPipelineInstanceRule : CollectionRule
    {
        /// <summary>
        /// Error message used when pipeline was not found.
        ///
        /// Supported placeholders:
        ///   Context,
        ///   TargetPipeline,
        ///   InvalidPipeline
        /// </summary>
        public string ErrorMessage { get; } = "Pipelines collection expected to contain pipeline instance '{TargetPipeline:F}'.";

        /// <summary>
        /// Pipeline to check.
        /// </summary>
        protected IPipeline TargetPipeline { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ContainsPipelineInstanceRule"/>.
        /// </summary>
        public ContainsPipelineInstanceRule(IPipeline targetPipeline) :
            base()
        {
            TargetPipeline = targetPipeline ?? throw new ArgumentNullException(nameof(targetPipeline));
        }

        /// <inheritdoc/>
        public override ValueTask ValidateAsync(RuleContext<IPipelineCollection> context, CancellationToken cancellationToken = default)
        {
            IPipeline? pipelineFromCollection = context.Pipelines.GetOrDefault(TargetPipeline.Name);

            if (pipelineFromCollection is null || !ReferenceEquals(pipelineFromCollection, TargetPipeline))
            {
                AddError(context,
                         ErrorMessage,
                         new Dictionary<string, object?>
                         {
                             ["Context"] = context,
                             ["TargetPipeline"] = TargetPipeline,
                             ["InvalidPipeline"] = pipelineFromCollection,
                         });
            }

            return default;
        }
    }
}

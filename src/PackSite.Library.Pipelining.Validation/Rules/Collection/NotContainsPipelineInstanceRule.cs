namespace PackSite.Library.Pipelining.Validation.Rules.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Validation.Validators;

    /// <summary>
    /// Validator that checks whether collection does not contain a specific pipeline instance in it.
    /// </summary>
    public class NotContainsPipelineInstanceRule : CollectionRule
    {
        /// <summary>
        /// Error message used when pipeline was not found.
        ///
        /// Supported placeholders:
        ///   Context,
        ///   TargetPipeline,
        ///   InvalidPipeline
        /// </summary>
        public string ErrorMessage { get; } = "Pipelines collection expected not to contain pipeline instance '{TargetPipeline:F}'.";

        /// <summary>
        /// Pipeline to check.
        /// </summary>
        protected IPipeline TargetPipeline { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="NotContainsPipelineInstanceRule"/>.
        /// </summary>
        public NotContainsPipelineInstanceRule(IPipeline targetPipeline) :
            base()
        {
            TargetPipeline = targetPipeline ?? throw new ArgumentNullException(nameof(targetPipeline));
        }

        /// <inheritdoc/>
        public override ValueTask ValidateAsync(RuleContext<IPipelineCollection> context, CancellationToken cancellationToken = default)
        {
            IPipeline? unexpectedPipeline = context.Pipelines.GetOrDefault(TargetPipeline.Name);

            if (unexpectedPipeline is not null && ReferenceEquals(unexpectedPipeline, TargetPipeline))
            {
                AddError(context,
                         ErrorMessage,
                         new Dictionary<string, object?>
                         {
                             ["Context"] = context,
                             ["TargetPipeline"] = TargetPipeline,
                             ["UnexpectedPipeline"] = unexpectedPipeline,
                         });
            }

            return default;
        }
    }
}

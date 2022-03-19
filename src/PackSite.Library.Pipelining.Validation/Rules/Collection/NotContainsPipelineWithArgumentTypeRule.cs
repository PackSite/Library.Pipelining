namespace PackSite.Library.Pipelining.Validation.Rules.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Validation.Validators;

    /// <summary>
    /// Validator that checks whether collection does not contain a pipeline with provided argument type in it.
    /// </summary>
    public class NotContainsPipelineWithArgumentTypeRule : CollectionRule
    {
        /// <summary>
        /// Error message used when pipeline was not found.
        ///
        /// Supported placeholders:
        ///   Context,
        ///   ArgumentType
        /// </summary>
        public string ErrorMessage { get; } = "Pipelines collection expected not to contain a pipeline with argument type '{ArgumentType}'.";

        /// <summary>
        /// Pipeline step argument type to check.
        /// </summary>
        protected Type ArgumentType { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="NotContainsPipelineWithArgumentTypeRule"/>.
        /// </summary>
        public NotContainsPipelineWithArgumentTypeRule(Type argumentType) :
            base()
        {
            ArgumentType = argumentType ?? throw new ArgumentNullException(nameof(argumentType));
        }

        /// <inheritdoc/>
        public override ValueTask ValidateAsync(RuleContext<IPipelineCollection> context, CancellationToken cancellationToken = default)
        {
            if (context.Pipelines.Where(x => x.ArgumentType == ArgumentType).FirstOrDefault() is IPipeline unexpectedPipeline)
            {
                AddError(context,
                         ErrorMessage,
                         new Dictionary<string, object?>
                         {
                             ["Context"] = context,
                             ["ArgumentType"] = ArgumentType,
                             ["UnexpectedPipeline"] = unexpectedPipeline,
                         });
            }

            return default;
        }
    }
}

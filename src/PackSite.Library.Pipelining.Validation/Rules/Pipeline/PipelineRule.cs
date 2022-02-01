namespace PackSite.Library.Pipelining.Validation.Rules.Pipeline
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Validation;
    using PackSite.Library.Pipelining.Validation.Validators;

    /// <summary>
    /// Represents a pipeline rule.
    /// </summary>
    public abstract class PipelineRule : IRule<IPipeline>
    {
        private readonly IPipeline _pipeline;

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineRule"/>.
        /// </summary>
        protected PipelineRule(IPipeline pipeline)
        {
            _pipeline = pipeline;
        }

        /// <inheritdoc/>
        public abstract ValueTask ValidateAsync(RuleContext<IPipeline> context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an error to context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="errorMessage"></param>
        /// <param name="formattedMessagePlaceholderValues"></param>
        /// <param name="customState"></param>
        /// <param name="errorCode"></param>
        protected static void AddError(RuleContext<IPipeline> context,
                                       string errorMessage,
                                       IDictionary<string, object?>? formattedMessagePlaceholderValues = null,
                                       object? customState = null,
                                       string? errorCode = null)
        {
            ValidationFailure failure = new(ValidationTarget.Pipeline, errorMessage)
            {
                CustomState = customState,
                ErrorCode = errorCode
            };

            if (formattedMessagePlaceholderValues is { Count: > 0 })
            {
                failure.FormattedMessagePlaceholderValues = formattedMessagePlaceholderValues;
            }

            context.Errors.Add(failure);
        }
    }
}

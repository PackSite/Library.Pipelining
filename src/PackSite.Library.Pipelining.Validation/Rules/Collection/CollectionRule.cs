namespace PackSite.Library.Pipelining.Validation.Rules.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Validation;
    using PackSite.Library.Pipelining.Validation.Validators;

    /// <summary>
    /// Represents a collection rule.
    /// </summary>
    public abstract class CollectionRule : IRule<IPipelineCollection>
    {
        /// <summary>
        /// Pipeline validator.
        /// </summary>
        protected ValidatorContext? SuchThatContext { get; set; } //TODO: rename to PipelineRulesContext

        /// <summary>
        /// Initializes a new instance of <see cref="CollectionRule"/>.
        /// </summary>
        protected CollectionRule()
        {

        }

        /// <summary>
        /// Pipeline validator.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws when And() operation is not supported for this context.</exception>
        public ValidatorContext SuchThat()
        {
            return SuchThatContext ?? throw new InvalidOperationException("And() operation is not supported for this context.");
        }

        /// <summary>
        /// Pipeline validator.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws when And() operation is not supported for this context.</exception>
        public ValidatorContext SuchThat(Action<ValidatorContext> action)
        {
            _ = SuchThatContext ?? throw new InvalidOperationException("And() operation is not supported for this context.");

            action(SuchThatContext);

            return SuchThatContext;
        }

        /// <inheritdoc/>
        public abstract ValueTask ValidateAsync(RuleContext<IPipelineCollection> context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds an error to context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="errorMessage"></param>
        /// <param name="formattedMessagePlaceholderValues"></param>
        /// <param name="customState"></param>
        /// <param name="errorCode"></param>
        protected static void AddError(RuleContext<IPipelineCollection> context,
                                       string errorMessage,
                                       IDictionary<string, object?>? formattedMessagePlaceholderValues = null,
                                       object? customState = null,
                                       string? errorCode = null)
        {
            ValidationFailure failure = new(ValidationTarget.Collection, errorMessage)
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

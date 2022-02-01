namespace PackSite.Library.Pipelining.Validation.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Inline validator.
    /// </summary>
    public class InlineValidator : IValidator
    {
        private readonly Func<ValidatorContext, CancellationToken, ValueTask> _validator;

        /// <summary>
        /// Initializes a new instance of <see cref="InlineValidator"/>.
        /// </summary>
        /// <param name="action"></param>
        public InlineValidator(Action<ValidatorContext> action)
        {
            _ = action ?? throw new ArgumentNullException(nameof(action));

            _validator = (context, ct) =>
            {
                action(context);

                return default;
            };
        }

        /// <summary>
        /// Initializes a new instance of <see cref="InlineValidator"/>.
        /// </summary>
        /// <param name="func"></param>
        public InlineValidator(Func<ValidatorContext, CancellationToken, ValueTask> func)
        {
            _validator = func ?? throw new ArgumentNullException(nameof(func));
        }

        /// <inheritdoc/>
        public virtual async ValueTask ValidateAsync(ValidatorContext context, CancellationToken cancellationToken = default)
        {
            await _validator(context, cancellationToken);
        }
    }
}

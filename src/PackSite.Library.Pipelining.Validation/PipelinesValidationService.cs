namespace PackSite.Library.Pipelining.Validation
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Validation.Internal.Extensions;
    using PackSite.Library.Pipelining.Validation.Internal.Services;
    using PackSite.Library.Pipelining.Validation.Validators;

    /// <summary>
    /// Default implementation of <see cref="IPipelinesValidationService"/>.
    /// </summary>
    public sealed class PipelinesValidationService : IPipelinesValidationService
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PipelinesValidationHostedService"/>.
        /// </summary>
        public PipelinesValidationService()
        {

        }

        /// <inheritdoc/>
        public async Task<ValidationResult> ValidateAsync(IPipelineCollection pipelines, IEnumerable<IValidator> validators, CancellationToken cancellationToken = default)
        {
            ValidationResult result = new();

            foreach (IValidator validator in validators)
            {
                ValidatorContext context = new(validator, pipelines);
                await validator.ValidateAsync(context, cancellationToken);

                result.Errors.AddRange(context.Errors);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task ValidateAndThrowAsync(IPipelineCollection pipelines, IEnumerable<IValidator> validators, CancellationToken cancellationToken = default)
        {
            ValidationResult validationResults = await ValidateAsync(pipelines, validators, cancellationToken);

            if (!validationResults.IsValid)
            {
                throw new ValidationException(validationResults);
            }
        }
    }
}

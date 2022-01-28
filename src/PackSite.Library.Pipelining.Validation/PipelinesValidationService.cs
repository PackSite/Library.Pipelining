namespace PackSite.Library.Pipelining.Validation
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Validation.Internal.Extensions;
    using PackSite.Library.Pipelining.Validation.Internal.Services;

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
        public async Task<ValidationResult> ValidateAsync(IPipelineCollection pipelines, IEnumerable<IPipelineValidator> validators, CancellationToken cancellationToken = default)
        {
            ValidationResult result = new();

            foreach (IPipelineValidator validator in validators)
            {
                ValidationContext context = new(validator, pipelines);
                await validator.ValidateAsync(context, cancellationToken);

                result.Errors.AddRange(validatorResult.Errors);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task ValidateAndThrow(IPipelineCollection pipelines, IEnumerable<IPipelineValidator> validators, CancellationToken cancellationToken = default)
        {
            ValidationResult validationResults = await ValidateAsync(pipelines, validators, cancellationToken);

            if (!validationResults.IsValid)
            {
                throw new PipelinesValidationException(validationResults);
            }
        }
    }
}

namespace PackSite.Library.Pipelining.Validation
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a service that validates pipelines.
    /// </summary>
    public partial interface IPipelinesValidationService
    {
        /// <summary>
        /// Validates pipelines in <paramref name="pipelines"/> using <paramref name="validators"/>.
        /// </summary>
        /// <param name="pipelines"></param>
        /// <param name="validators"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ValidationResult> ValidateAsync(IPipelineCollection pipelines, IEnumerable<IValidator> validators, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates pipelines in <paramref name="pipelines"/> using <paramref name="validators"/>.
        /// </summary>
        /// <param name="pipelines"></param>
        /// <param name="validators"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="PipelinesValidationException">Throws when pipelines validaiton fails for one or more pipelines.</exception>
        Task ValidateAndThrowAsync(IPipelineCollection pipelines, IEnumerable<IValidator> validators, CancellationToken cancellationToken = default);
    }
}
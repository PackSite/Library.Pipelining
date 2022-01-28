namespace PackSite.Library.Pipelining.Validation
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a pipeline validator.
    /// </summary>
    public interface IPipelineValidator
    {
        /// <summary>
        /// Validates pipeline from the collection.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ValidateAsync(ValidationContext context, CancellationToken cancellationToken = default);
    }
}

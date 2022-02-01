namespace PackSite.Library.Pipelining.Validation.Validators
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a validator.
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Validates a pipeline from the collection or a collection itself.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ValidateAsync(ValidatorContext context, CancellationToken cancellationToken = default);
    }
}

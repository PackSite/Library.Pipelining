namespace PackSite.Library.Pipelining.Validation.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Collection validator.
    /// </summary>
    public class CollectionValidator : IValidator
    {
        /// <inheritdoc/>
        public ValueTask ValidateAsync(ValidationContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

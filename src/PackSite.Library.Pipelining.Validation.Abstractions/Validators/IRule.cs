namespace PackSite.Library.Pipelining.Validation.Validators
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a validator rule.
    /// </summary>
    public interface IRule<T>
        where T : class
    {
        /// <summary>
        /// Validates the rule.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ValidateAsync(RuleContext<T> context, CancellationToken cancellationToken = default);
    }
}

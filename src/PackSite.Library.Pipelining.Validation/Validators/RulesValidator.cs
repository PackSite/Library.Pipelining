namespace PackSite.Library.Pipelining.Validation.Validators
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Rules validator.
    /// </summary>
    public abstract class RulesValidator : IValidator
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RulesValidator"/>.
        /// </summary>
        public RulesValidator()
        {

        }

        /// <inheritdoc/>
        public virtual async ValueTask ValidateAsync(ValidatorContext context, CancellationToken cancellationToken = default)
        {
            RuleContext<IPipelineCollection> ruleContext = new(context, context.Pipelines);

            await BuildRulesAsync(ruleContext, cancellationToken);

            foreach (IRule<IPipelineCollection> rule in ruleContext.ChildRules)
            {
                await rule.ValidateAsync(ruleContext, cancellationToken);
            }
        }

        /// <summary>
        /// Validates using rules.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract ValueTask BuildRulesAsync(RuleContext<IPipelineCollection> context, CancellationToken cancellationToken = default);
    }
}

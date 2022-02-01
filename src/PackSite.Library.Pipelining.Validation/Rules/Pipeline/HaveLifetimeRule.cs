namespace PackSite.Library.Pipelining.Validation.Rules.Pipeline
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Validation.Validators;

    /// <summary>
    /// Validator that checks pipeline lifetime.
    /// </summary>
    public class HaveLifetimeRule : PipelineRule
    {
        /// <summary>
        /// Error message used when pipeline lifetime is invalid.
        ///
        /// Supported placeholders:
        ///   Context,
        ///   Operator,
        ///   CurrentValue,
        ///   TestValue
        /// </summary>
        public string ErrorMessage { get; } = "Pipeline '{Pipeline:f}' expected to have lifetime equal to '{TestValue}', but was '{CurrentValue}'.";

        /// <summary>
        /// Expected lifetime.
        /// </summary>
        protected InvokablePipelineLifetime Lifetime { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="HaveLifetimeRule"/>.
        /// </summary>
        public HaveLifetimeRule(IPipeline pipeline, InvokablePipelineLifetime expectedLifetime) :
            base(pipeline)
        {
            Lifetime = expectedLifetime;
        }

        /// <inheritdoc/>
        public override ValueTask ValidateAsync(RuleContext<IPipeline> context, CancellationToken cancellationToken = default)
        {
            if (context.Instance.Lifetime != Lifetime)
            {
                AddError(context,
                         ErrorMessage,
                         new Dictionary<string, object?>
                         {
                             ["Context"] = context,
                             ["Pipeline"] = context.Instance,
                             ["CurrentValue"] = context.Instance.Lifetime,
                             ["TestValue"] = Lifetime,
                         });
            }

            return default;
        }
    }
}

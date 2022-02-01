namespace PackSite.Library.Pipelining.Validation.Validators.Pipeline
{
    using PackSite.Library.Pipelining.Validation.Rules.Pipeline;

    /// <summary>
    /// Pipeline validator extensions.
    /// </summary>
    public static partial class PipelineRulesExtensions
    {
        /// <summary>
        /// Verifies pipeline lifetime.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="lifetime"></param>
        public static HaveLifetimeRule HaveLifetime(this RuleContext<IPipeline> context, InvokablePipelineLifetime lifetime)
        {
            HaveLifetimeRule validator = new(context.Instance, lifetime);

            return validator;
        }

        /// <summary>
        /// Verifies pipeline lifetime.
        /// </summary>
        /// <param name="context"></param>
        public static HaveLifetimeRule HaveSingletonLifetime(this RuleContext<IPipeline> context)
        {
            return context.HaveLifetime(InvokablePipelineLifetime.Singleton);
        }

        /// <summary>
        /// Verifies pipeline lifetime.
        /// </summary>
        /// <param name="context"></param>
        public static HaveLifetimeRule HaveScopedLifetime(this RuleContext<IPipeline> context)
        {
            return context.HaveLifetime(InvokablePipelineLifetime.Scoped);
        }

        /// <summary>
        /// Verifies pipeline lifetime.
        /// </summary>
        /// <param name="context"></param>
        public static HaveLifetimeRule HaveTransientLifetime(this RuleContext<IPipeline> context)
        {
            return context.HaveLifetime(InvokablePipelineLifetime.Transient);
        }
    }
}

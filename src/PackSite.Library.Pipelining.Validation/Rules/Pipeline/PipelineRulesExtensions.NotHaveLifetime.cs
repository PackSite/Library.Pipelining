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
        public static NotHaveLifetimeRule NotHaveLifetime(this RuleContext<IPipeline> context, InvokablePipelineLifetime lifetime)
        {
            NotHaveLifetimeRule validator = new(context.Instance, lifetime);

            return validator;
        }

        /// <summary>
        /// Verifies pipeline lifetime.
        /// </summary>
        /// <param name="context"></param>
        public static NotHaveLifetimeRule NotHaveSingletonLifetime(this RuleContext<IPipeline> context)
        {
            return context.NotHaveLifetime(InvokablePipelineLifetime.Singleton);
        }

        /// <summary>
        /// Verifies pipeline lifetime.
        /// </summary>
        /// <param name="context"></param>
        public static NotHaveLifetimeRule NotHaveScopedLifetime(this RuleContext<IPipeline> context)
        {
            return context.NotHaveLifetime(InvokablePipelineLifetime.Scoped);
        }

        /// <summary>
        /// Verifies pipeline lifetime.
        /// </summary>
        /// <param name="context"></param>
        public static NotHaveLifetimeRule NotHaveTransientLifetime(this RuleContext<IPipeline> context)
        {
            return context.NotHaveLifetime(InvokablePipelineLifetime.Transient);
        }
    }
}

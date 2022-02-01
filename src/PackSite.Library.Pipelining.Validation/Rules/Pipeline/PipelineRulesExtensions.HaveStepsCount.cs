namespace PackSite.Library.Pipelining.Validation.Validators.Pipeline
{
    using PackSite.Library.Pipelining.Validation.Rules;
    using PackSite.Library.Pipelining.Validation.Rules.Pipeline;

    /// <summary>
    /// Pipeline validator extensions.
    /// </summary>
    public static partial class PipelineRulesExtensions
    {
        /// <summary>
        /// Verifies whether pipeline has steps.
        /// </summary>
        /// <param name="context"></param>
        public static HaveStepsCountRule HaveSteps(this RuleContext<IPipeline> context)
        {
            return context.HaveStepsCountNotEqualTo(0);
        }

        /// <summary>
        /// Verifies pipeline steps count.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="operator"></param>
        /// <param name="operand"></param>
        public static HaveStepsCountRule HaveStepsCount(this RuleContext<IPipeline> context, Operator @operator, int operand)
        {
            HaveStepsCountRule validator = new(context.Instance, @operator, operand);

            return validator;
        }

        /// <summary>
        /// Verifies whether pipeline steps count is less than <paramref name="operand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="operand"></param>
        public static HaveStepsCountRule HaveStepsCountLessThan(this RuleContext<IPipeline> context, int operand)
        {
            return context.HaveStepsCount(Operator.LessThan, operand);
        }

        /// <summary>
        /// Verifies whether pipeline steps count is less than <paramref name="operand"/> or equal to <paramref name="operand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="operand"></param>
        public static HaveStepsCountRule HaveStepsCountLessThanOrEqualTo(this RuleContext<IPipeline> context, int operand)
        {
            return context.HaveStepsCount(Operator.LessThanOrEqualTo, operand);
        }

        /// <summary>
        /// Verifies whether pipeline steps count is equal to <paramref name="operand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="operand"></param>
        public static HaveStepsCountRule HaveStepsCountEqualTo(this RuleContext<IPipeline> context, int operand)
        {
            return context.HaveStepsCount(Operator.EqualTo, operand);
        }

        /// <summary>
        /// Verifies whether pipeline steps count is not equal to <paramref name="operand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="operand"></param>
        public static HaveStepsCountRule HaveStepsCountNotEqualTo(this RuleContext<IPipeline> context, int operand)
        {
            return context.HaveStepsCount(Operator.NotEqualTo, operand);
        }

        /// <summary>
        /// Verifies whether pipeline steps count is greater than <paramref name="operand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="operand"></param>
        public static HaveStepsCountRule HaveStepsCountGreaterThan(this RuleContext<IPipeline> context, int operand)
        {
            return context.HaveStepsCount(Operator.GreaterThan, operand);
        }

        /// <summary>
        /// Verifies whether pipeline steps count is greater than <paramref name="operand"/> or equal to <paramref name="operand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="operand"></param>
        public static HaveStepsCountRule HaveStepsCountGreaterThanOrEqualTo(this RuleContext<IPipeline> context, int operand)
        {
            return context.HaveStepsCount(Operator.GreaterThanOrEqualTo, operand);
        }
    }
}

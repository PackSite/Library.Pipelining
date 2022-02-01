namespace PackSite.Library.Pipelining.Validation.Validators
{
    using PackSite.Library.Pipelining.Validation.Rules;
    using PackSite.Library.Pipelining.Validation.Rules.Collection;

    /// <summary>
    /// Collection validator extensions.
    /// </summary>
    public static partial class CollectionRulesExtensions
    {
        /// <summary>
        /// Verifies whether pipelines collection is empty.
        /// </summary>
        /// <param name="context"></param>
        public static HaveCountRule BeEmpty(this ValidatorContext context)
        {
            return context.HaveCountEqualTo(0);
        }

        /// <summary>
        /// Verifies whether pipelines collection is not empty.
        /// </summary>
        /// <param name="context"></param>
        public static HaveCountRule NotBeEmpty(this ValidatorContext context)
        {
            return context.HaveCountNotEqualTo(0);
        }

        /// <summary>
        /// Verifies pipelines collection count.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="operator"></param>
        /// <param name="operand"></param>
        public static HaveCountRule HaveCount(this ValidatorContext context, Operator @operator, int operand)
        {
            HaveCountRule validator = new(@operator, operand);

            return validator;
        }

        /// <summary>
        /// Verifies whether pipelines collection count is less than <paramref name="operand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="operand"></param>
        public static HaveCountRule HaveCountLessThan(this ValidatorContext context, int operand)
        {
            return context.HaveCount(Operator.LessThan, operand);
        }

        /// <summary>
        /// Verifies whether pipelines collection count is less than <paramref name="operand"/> or equal to <paramref name="operand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="operand"></param>
        public static HaveCountRule HaveCountLessThanOrEqualTo(this ValidatorContext context, int operand)
        {
            return context.HaveCount(Operator.LessThanOrEqualTo, operand);
        }

        /// <summary>
        /// Verifies whether pipelines collection count is equal to <paramref name="operand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="operand"></param>
        public static HaveCountRule HaveCountEqualTo(this ValidatorContext context, int operand)
        {
            return context.HaveCount(Operator.EqualTo, operand);
        }

        /// <summary>
        /// Verifies whether pipelines collection count is not equal to <paramref name="operand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="operand"></param>
        public static HaveCountRule HaveCountNotEqualTo(this ValidatorContext context, int operand)
        {
            return context.HaveCount(Operator.NotEqualTo, operand);
        }

        /// <summary>
        /// Verifies whether pipelines collection count is greater than <paramref name="operand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="operand"></param>
        public static HaveCountRule HaveCountGreaterThan(this ValidatorContext context, int operand)
        {
            return context.HaveCount(Operator.GreaterThan, operand);
        }

        /// <summary>
        /// Verifies whether pipelines collection count is greater than <paramref name="operand"/> or equal to <paramref name="operand"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="operand"></param>
        public static HaveCountRule HaveCountGreaterThanOrEqualTo(this ValidatorContext context, int operand)
        {
            return context.HaveCount(Operator.GreaterThanOrEqualTo, operand);
        }
    }
}

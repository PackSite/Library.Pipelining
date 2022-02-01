namespace PackSite.Library.Pipelining.Validation.Rules.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Validation.Rules;
    using PackSite.Library.Pipelining.Validation.Validators;

    /// <summary>
    /// Validator that checks pipeline steps count.
    /// </summary>
    public class HaveStepsCountRule : PipelineRule
    {
        /// <summary>
        /// Error message used when pipeline steps count does not meat the criteria.
        ///
        /// Supported placeholders:
        ///   Context,
        ///   Pipeline,
        ///   Operator,
        ///   Operand
        /// </summary>
        public string ErrorMessage { get; } = "Pipeline '{Pipeline:f}' expected to have 'Steps.Count {Operator} {Operand}'.";

        /// <summary>
        /// Operator to use.
        /// </summary>
        protected Operator Operator { get; }

        /// <summary>
        /// Operand.
        /// </summary>
        protected int Operand { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="HaveStepsCountRule"/>.
        /// </summary>
        public HaveStepsCountRule(IPipeline pipeline, Operator @operator, int operand) :
            base(pipeline)
        {
            Operator = @operator;
            Operand = operand;
        }

        /// <inheritdoc/>
        public override ValueTask ValidateAsync(RuleContext<IPipeline> context, CancellationToken cancellationToken = default)
        {
            int stepsCount = context.Instance.Steps.Count;

            bool test = Operator switch
            {
                Operator.LessThan => stepsCount < Operand,
                Operator.LessThanOrEqualTo => stepsCount <= Operand,
                Operator.EqualTo => stepsCount == Operand,
                Operator.NotEqualTo => stepsCount != Operand,
                Operator.GreaterThan => stepsCount > Operand,
                Operator.GreaterThanOrEqualTo => stepsCount >= Operand,

                _ => throw new InvalidOperationException($"The {Operator} operator is not supported."),
            };

            if (!test)
            {
                AddError(context,
                         ErrorMessage,
                         new Dictionary<string, object?>
                         {
                             ["Context"] = context,
                             ["Pipeline"] = context.Instance,
                             ["Operator"] = Operator,
                             ["Operand"] = Operand,
                         });
            }

            return default;
        }
    }
}

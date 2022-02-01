namespace PackSite.Library.Pipelining.Validation.Rules.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Validation.Rules;
    using PackSite.Library.Pipelining.Validation.Validators;

    /// <summary>
    /// Validator that checks collection count.
    /// </summary>
    public class HaveCountRule : CollectionRule
    {
        /// <summary>
        /// Error message used when pipeline collection count does not meat the criteria.
        ///
        /// Supported placeholders:
        ///   Context,
        ///   Operator,
        ///   Operand
        /// </summary>
        public string ErrorMessage { get; } = "Pipelines collection expected to have 'Count {Operator} {Operand}'.";

        /// <summary>
        /// Operator to use.
        /// </summary>
        protected Operator Operator { get; }

        /// <summary>
        /// Operand.
        /// </summary>
        protected int Operand { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="HaveCountRule"/>.
        /// </summary>
        public HaveCountRule(Operator @operator, int operand) :
            base()
        {
            Operator = @operator;
            Operand = operand;
        }

        /// <inheritdoc/>
        public override ValueTask ValidateAsync(RuleContext<IPipelineCollection> context, CancellationToken cancellationToken = default)
        {
            int pipelinesCount = context.Pipelines.Count;

            bool test = Operator switch
            {
                Operator.LessThan => pipelinesCount < Operand,
                Operator.LessThanOrEqualTo => pipelinesCount <= Operand,
                Operator.EqualTo => pipelinesCount == Operand,
                Operator.NotEqualTo => pipelinesCount != Operand,
                Operator.GreaterThan => pipelinesCount > Operand,
                Operator.GreaterThanOrEqualTo => pipelinesCount >= Operand,

                _ => throw new InvalidOperationException($"The {Operator} operator is not supported."),
            };

            if (!test)
            {
                AddError(context,
                         ErrorMessage,
                         new Dictionary<string, object?>
                         {
                             ["Context"] = context,
                             ["Operator"] = Operator,
                             ["Operand"] = Operand,
                         });
            }

            return default;
        }
    }
}

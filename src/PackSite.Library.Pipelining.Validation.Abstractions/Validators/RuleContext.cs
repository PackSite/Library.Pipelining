namespace PackSite.Library.Pipelining.Validation.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using PackSite.Library.Pipelining.Validation.Collection;

    /// <summary>
    /// Pipeline validation context.
    /// </summary>
    public class RuleContext<T> : IFormattable
        where T : class
    {
        /// <summary>
        /// Parent validator context.
        /// </summary>
        public ValidatorContext Validator { get; }

        /// <summary>
        /// A collection of all pipelines.
        /// </summary>
        public IPipelineCollection Pipelines { get; }

        /// <summary>
        /// Whether validation succeeded.
        /// </summary>
        public bool IsValid => Errors.Count == 0;

        /// <summary>
        /// A collection of errors
        /// </summary>
        public IList<ValidationFailure> Errors { get; }

        /// <summary>
        /// A collection of errors
        /// </summary>
        public IList<IRule<T>> ChildRules { get; } = new List<IRule<T>>();

        /// <summary>
        /// Pipeline instance being validated.
        /// </summary>
        public T Instance { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="RuleContext{T}"/>.
        /// </summary>
        /// <param name="validatorContext"></param>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RuleContext(ValidatorContext validatorContext, T instance)
        {
            Validator = validatorContext;
            Pipelines = validatorContext.Pipelines;
            Errors = new SubList<ValidationFailure>(validatorContext.Errors);
            Instance = instance;
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return ToString(null, null);
        }

        /// <inheritdoc/>
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                format = "d";
            }

            formatProvider ??= CultureInfo.CurrentCulture;

            return format.ToLowerInvariant() switch
            {
                "d" or "default" => base.ToString() ?? string.Empty,

                "i" or "instance" => Instance is IFormattable i
                    ? i.ToString(null, formatProvider)
                    : Instance.ToString() ?? string.Empty,

                "p" or "pipelines" => Pipelines.ToString() ?? string.Empty,

                "v" or "validator" => Validator is IFormattable f
                    ? f.ToString(null, formatProvider)
                    : Validator.ToString() ?? string.Empty,

                _ => throw new FormatException($"The {format} format string is not supported."),
            };
        }
    }
}

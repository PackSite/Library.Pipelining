namespace PackSite.Library.Pipelining.Validation.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Pipelining validation context.
    /// </summary>
    public class ValidatorContext : IFormattable
    {
        /// <summary>
        /// Validator instance.
        /// </summary>
        public IValidator Validator { get; }

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
        public IList<ValidationFailure> Errors { get; } = new List<ValidationFailure>();

        /// <summary>
        /// Initializes a new instance of <see cref="ValidatorContext"/>.
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="pipelines"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ValidatorContext(IValidator validator, IPipelineCollection pipelines)
        {
            Validator = validator;
            Pipelines = pipelines;
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
                format = "n";
            }

            formatProvider ??= CultureInfo.CurrentCulture;

            return format.ToLowerInvariant() switch
            {
                "d" or "default" => base.ToString() ?? string.Empty,
                "p" or "pipelines" => Pipelines.ToString() ?? string.Empty,
                "v" or "validator" => Validator is IFormattable f
                    ? f.ToString(null, formatProvider)
                    : Validator.ToString() ?? string.Empty,

                _ => throw new FormatException($"The {format} format string is not supported."),
            };
        }
    }
}

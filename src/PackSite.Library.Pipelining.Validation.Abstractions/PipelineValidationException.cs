namespace PackSite.Library.Pipelining.Validation
{
    using System;

    /// <summary>
    /// An exception thrown when pipelines validation fails for one or more pipelines.
    /// </summary>
    public class PipelinesValidationException : Exception
    {
        /// <summary>
        /// Validation result.
        /// </summary>
        public ValidationResult ValidationResult { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="PipelinesValidationException"/>.
        /// </summary>
        public PipelinesValidationException(ValidationResult validationResult) :
            base($"Pipelines validation failed.")
        {
            ValidationResult = validationResult;
        }
    }
}

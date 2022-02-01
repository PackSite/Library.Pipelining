namespace PackSite.Library.Pipelining.Validation
{
    using System;

    /// <summary>
    /// An exception thrown when pipelines validation fails for one or more pipelines.
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Validation result.
        /// </summary>
        public ValidationResult ValidationResult { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ValidationException"/>.
        /// </summary>
        public ValidationException(ValidationResult validationResult) :
            base($"Pipelines validation failed.")
        {
            ValidationResult = validationResult;
        }
    }
}

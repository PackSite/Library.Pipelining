namespace PackSite.Library.Pipelining.Validation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a pipeline validation failure aka. error.
    /// </summary>
    /// <summary>
    /// Defines a validation failure
    /// </summary>
    [Serializable]
    public class ValidationFailure
    {
        /// <summary>
        /// The validation target.
        /// </summary>
        public ValidationTarget Target { get; }

        /// <summary>
        /// The error message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the formatted message placeholder values.
        /// </summary>
        public IDictionary<string, object?> FormattedMessagePlaceholderValues { get; set; } = new Dictionary<string, object?>();

        /// <summary>
        /// Custom state associated with the failure.
        /// </summary>
        public object? CustomState { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Creates a new ValidationFailure.
        /// </summary>
        public ValidationFailure(ValidationTarget target, string errorMessage)
        {
            Target = target;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Creates a textual representation of the failure.
        /// </summary>
        public override string ToString()
        {
            return ErrorMessage;
        }
    }
}
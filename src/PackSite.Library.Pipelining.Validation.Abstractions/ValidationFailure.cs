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
		public ValidationTarget Target { get; set; }

		/// <summary>
		/// The error message
		/// </summary>
		public string ErrorMessage { get; set; }

		/// <summary>
		/// The property value that caused the failure.
		/// </summary>
		public object AttemptedValue { get; set; }

		/// <summary>
		/// Custom state associated with the failure.
		/// </summary>
		public object CustomState { get; set; }

		/// <summary>
		/// Custom severity level associated with the failure.
		/// </summary>
		public Severity Severity { get; set; } = Severity.Error;

		/// <summary>
		/// Gets or sets the error code.
		/// </summary>
		public string ErrorCode { get; set; }

		/// <summary>
		/// Gets or sets the formatted message placeholder values.
		/// </summary>
		public Dictionary<string, object> FormattedMessagePlaceholderValues { get; set; }

		/// <summary>
		/// Creates a new ValidationFailure.
		/// </summary>
		public ValidationFailure(ValidationTarget target, string errorMessage, object attemptedValue)
		{
			Target = target;
			ErrorMessage = errorMessage;
			AttemptedValue = attemptedValue;
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
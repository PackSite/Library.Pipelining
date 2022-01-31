namespace PackSite.Library.Pipelining.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Pipeline validation results returned by <see cref="IPipelinesValidationService"/>
    /// </summary>
    public sealed class ValidationResult
    {
        /// <summary>
        /// Whether validation succeeded.
        /// </summary>
        public bool IsValid => Errors.Count == 0;

        /// <summary>
        /// A collection of errors
        /// </summary>
        public IList<ValidationFailure> Errors { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ValidationResult"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public ValidationResult()
        {
            Errors = new List<ValidationFailure>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="errors"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ValidationResult(IEnumerable<ValidationFailure> errors)
        {
            Errors = errors.ToList();
        }
    }
}

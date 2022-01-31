namespace PackSite.Library.Pipelining.Validation
{
    using System;
    using System.Collections.Generic;

    //TODO: add CollectionValidationContext, PipelineValidationContext, StepValidationContext

    /// <summary>
    /// Pipeline validation results returned by <see cref="IPipelinesValidationService"/>
    /// </summary>
    public sealed class ValidationContext
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
        public IList<ValidationFailure> Errors { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="pipelines"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ValidationContext(IValidator validator, IPipelineCollection pipelines)
        {
            Validator = validator;
            Pipelines = pipelines;
            Errors = new List<ValidationFailure>();
        }
    }
}

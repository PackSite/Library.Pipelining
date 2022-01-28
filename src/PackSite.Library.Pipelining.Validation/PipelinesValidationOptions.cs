namespace PackSite.Library.Pipelining.Validation
{
    /// <summary>
    /// Pipelines validationo options.
    /// </summary>
    public sealed class PipelinesValidationOptions
    {
        /// <summary>
        /// Whether to validate pipelines on startup (default: true).
        /// </summary>
        public bool ValidateOnStartup { get; set; } = true;

        /// <summary>
        /// Whether to validate pipelines on colection change (default: true).
        /// </summary>
        public bool ValidateOnCollectionChange { get; set; } = true;
    }
}

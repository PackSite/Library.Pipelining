namespace PackSite.Library.Pipelining.Validation
{
    /// <summary>
    /// Pipelines validation options.
    /// </summary>
    public sealed class PipelinesValidationOptions
    {
        /// <summary>
        /// Whether to validate pipelines on startup (default: true).
        /// </summary>
        public bool ValidateOnStartup { get; set; } = true;

        /// <summary>
        /// Whether to validate pipelines on collection change (default: true).
        /// </summary>
        public bool ValidateOnCollectionChange { get; set; } = true;
    }
}

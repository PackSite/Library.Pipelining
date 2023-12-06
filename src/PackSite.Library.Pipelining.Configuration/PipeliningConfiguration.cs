namespace PackSite.Library.Pipelining.Configuration
{
    /// <summary>
    /// Pipelining configuration.
    /// </summary>
    public sealed class PipeliningConfiguration
    {
        /// <summary>
        /// Whether to throw on config reload error.
        /// </summary>
        public bool ThrowOnReloadError { get; set; }

        /// <summary>
        /// Pipeline definitions map (key is a pipeline name).
        /// </summary>
        public Dictionary<string, PipelineDefinition?>? Pipelines { get; set; }
    }
}

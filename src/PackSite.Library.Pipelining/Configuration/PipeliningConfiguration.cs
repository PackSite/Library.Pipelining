namespace PackSite.Library.Pipelining.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// Pipelining configuration.
    /// </summary>
    public sealed class PipeliningConfiguration
    {
        /// <summary>
        /// Whether pipeline profiling is enabled.
        /// </summary>
        public bool EnableProfiling { get; set; }

        /// <summary>
        /// Whether to throw on cofig reload error.
        /// </summary>
        public bool ThrowOnReloadError { get; set; }

        /// <summary>
        /// Pipeline definitions.
        /// </summary>
        public List<PipelineDefinition>? Pipelines { get; set; }
    }
}

namespace PackSite.Library.Pipelining
{
    using System;

    /// <summary>
    /// Arguments of the event invoked when a pipeline was updated in <see cref="IPipelineCollection"/>.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of <see cref="PipelineAddedEventArgs"/>.
    /// </remarks>
    /// <param name="pipelineName"></param>
    public sealed class PipelineUpdatedEventArgs(
        PipelineName pipelineName) : EventArgs
    {
        /// <summary>
        /// Pipeline name.
        /// </summary>
        public PipelineName PipelineName { get; } = pipelineName ?? throw new ArgumentNullException(nameof(pipelineName));
    }
}
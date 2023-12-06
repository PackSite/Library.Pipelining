namespace PackSite.Library.Pipelining
{
    /// <summary>
    /// Arguments of the event invoked when a pipeline was removed from <see cref="IPipelineCollection"/>.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of <see cref="PipelineRemovedEventArgs"/>.
    /// </remarks>
    /// <param name="pipelineName"></param>
    public sealed class PipelineRemovedEventArgs(
        PipelineName pipelineName) : EventArgs
    {
        /// <summary>
        /// Pipeline name.
        /// </summary>
        public PipelineName PipelineName { get; } = pipelineName ?? throw new ArgumentNullException(nameof(pipelineName));
    }
}
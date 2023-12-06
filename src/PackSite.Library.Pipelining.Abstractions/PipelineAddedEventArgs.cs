namespace PackSite.Library.Pipelining
{
    /// <summary>
    /// Arguments of the event invoked when a pipeline was added to <see cref="IPipelineCollection"/>.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of <see cref="PipelineAddedEventArgs"/>.
    /// </remarks>
    /// <param name="pipelineName"></param>
    public sealed class PipelineAddedEventArgs(
        PipelineName pipelineName) : EventArgs
    {
        /// <summary>
        /// Pipeline name.
        /// </summary>
        public PipelineName PipelineName { get; } = pipelineName ?? throw new ArgumentNullException(nameof(pipelineName));
    }
}
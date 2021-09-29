namespace PackSite.Library.Pipelining
{
    using System;

    /// <summary>
    /// Arguments of the event invoked when a pipeline was removed from <see cref="IPipelineCollection"/>.
    /// </summary>
    public sealed class PipelineRemovedEventArgs : EventArgs
    {
        /// <summary>
        /// Pipeline name.
        /// </summary>
        public PipelineName PipelineName { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineRemovedEventArgs"/>.
        /// </summary>
        /// <param name="pipelineName"></param>
        public PipelineRemovedEventArgs(PipelineName pipelineName)
        {
            PipelineName = pipelineName ?? throw new ArgumentNullException(nameof(pipelineName));
        }
    }
}
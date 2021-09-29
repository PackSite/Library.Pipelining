namespace PackSite.Library.Pipelining
{
    using System;

    /// <summary>
    /// Arguments of the event invoked when a pipeline was updated in <see cref="IPipelineCollection"/>.
    /// </summary>
    public sealed class PipelineUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// Pipeline name.
        /// </summary>
        public PipelineName PipelineName { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineAddedEventArgs"/>.
        /// </summary>
        /// <param name="pipelineName"></param>
        public PipelineUpdatedEventArgs(PipelineName pipelineName)
        {
            PipelineName = pipelineName ?? throw new ArgumentNullException(nameof(pipelineName));
        }
    }
}
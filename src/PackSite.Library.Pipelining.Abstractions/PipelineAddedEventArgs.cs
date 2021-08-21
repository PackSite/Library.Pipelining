namespace PackSite.Library.Pipelining
{
    using System;

    /// <summary>
    /// Arguments of the event invoked when a pipeline was added to <see cref="IPipelineCollection"/>.
    /// </summary>
    public class PipelineAddedEventArgs : EventArgs
    {
        /// <summary>
        /// Pipeline name.
        /// </summary>
        public PipelineName PipelineName { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineAddedEventArgs"/>.
        /// </summary>
        /// <param name="pipelineName"></param>
        public PipelineAddedEventArgs(PipelineName pipelineName)
        {
            PipelineName = pipelineName ?? throw new ArgumentNullException(nameof(pipelineName));
        }
    }
}
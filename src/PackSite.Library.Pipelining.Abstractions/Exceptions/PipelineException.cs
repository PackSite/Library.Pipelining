namespace PackSite.Library.Pipelining.Exceptions
{
    using System;

    /// <summary>
    /// Pipeline exception exception.
    /// </summary>
    public sealed class PipelineException : Exception
    {
        /// <summary>
        /// Pipeline input context.
        /// </summary>
        public object? Context { get; }

        /// <summary>
        /// Pipeline.
        /// </summary>
        public IPipeline Pipeline { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineException"/>.
        /// </summary>
        /// <param name="inputContext"></param>
        /// <param name="pipeline"></param>
        /// <param name="innerException"></param>
        public PipelineException(object? inputContext, IPipeline pipeline, Exception? innerException) :
            base($"An unhandled error occured while executing pipeline '{pipeline.GetType().FullName ?? pipeline.GetType().Name}' with name '{pipeline.Name}'.", innerException)
        {
            Context = inputContext;
            Pipeline = pipeline;
        }
    }
}

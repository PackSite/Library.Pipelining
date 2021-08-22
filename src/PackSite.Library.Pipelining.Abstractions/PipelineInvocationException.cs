namespace PackSite.Library.Pipelining
{
    using System;

    /// <summary>
    /// Pipeline invocation exception.
    /// </summary>
    public sealed class PipelineInvocationException : Exception
    {
        /// <summary>
        /// Pipeline args.
        /// </summary>
        public object? Args { get; }

        /// <summary>
        /// Pipeline.
        /// </summary>
        public IPipeline Pipeline { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineInvocationException"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="pipeline"></param>
        /// <param name="innerException"></param>
        public PipelineInvocationException(object? args, IPipeline pipeline, Exception? innerException) :
            base($"An unhandled error occured while executing pipeline '{pipeline.GetType().FullName ?? pipeline.GetType().Name}' with name '{pipeline.Name}'.", innerException)
        {
            Args = args;
            Pipeline = pipeline;
        }
    }
}

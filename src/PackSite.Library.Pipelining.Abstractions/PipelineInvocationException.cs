namespace PackSite.Library.Pipelining
{
    /// <summary>
    /// Pipeline invocation exception.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of <see cref="PipelineInvocationException"/>.
    /// </remarks>
    /// <param name="args"></param>
    /// <param name="pipeline"></param>
    /// <param name="innerException"></param>
    public sealed class PipelineInvocationException(
        object? args,
        IPipeline pipeline,
        Exception? innerException) : 
        Exception($"An unhandled error occurred while executing pipeline '{pipeline.GetType().FullName ?? pipeline.GetType().Name}' with name '{pipeline.Name}'.", innerException)
    {
        /// <summary>
        /// Pipeline args.
        /// </summary>
        public object? Args { get; } = args;

        /// <summary>
        /// Pipeline.
        /// </summary>
        public IPipeline Pipeline { get; } = pipeline;
    }
}

namespace PackSite.Library.Pipelining.Exceptions
{
    using System;

    /// <summary>
    /// Item serialization exception.
    /// </summary>
    public sealed class PipelineException : Exception
    {
        /// <inheritdoc/>
        public PipelineException(IPipeline pipeline, Exception? innerException) :
            base($"An unhandled error occured while executing pipeline '{pipeline.GetType().FullName ?? pipeline.GetType().Name}' with name '{pipeline.Name}'.", innerException)
        {

        }
    }
}

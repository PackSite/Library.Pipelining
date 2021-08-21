namespace PackSite.Library.Pipelining
{
    /// <summary>
    /// <see cref="IPipeline"/> extensions.
    /// </summary>
    public static class PipelineExtensions
    {
        /// <summary>
        /// Tries to add a pipeline to a collection.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <param name="pipelineCollection"></param>
        /// <returns></returns>
        public static bool TryAddTo(this IPipeline pipeline, IPipelineCollection pipelineCollection)
        {
            return pipelineCollection.TryAdd(pipeline);
        }

        /// <summary>
        /// Tries to add a pipeline to a collection.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <param name="pipelineCollection"></param>
        /// <param name="out"></param>
        /// <returns></returns>
        public static bool TryAddTo(this IPipeline pipeline, IPipelineCollection pipelineCollection, out IPipeline @out)
        {
            @out = pipeline;

            return pipelineCollection.TryAdd(pipeline);
        }
    }
}

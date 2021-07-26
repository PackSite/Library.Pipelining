namespace PackSite.Library.Pipelining
{
    using System.Collections.Generic;

    /// <summary>
    /// Pipeline collection.
    /// </summary>
    public interface IPipelineCollection : IReadOnlyCollection<IPipeline>
    {
        /// <summary>
        /// Pipeline names.
        /// </summary>
        IReadOnlyList<PipelineName> Names { get; }

        /// <summary>
        /// Attempts to add the specified pipeline.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <returns>true if pipeline was added successfully; otherwise, false.</returns>
        bool TryAdd(IPipeline pipeline);

        /// <summary>
        /// Attempts to remove the specified pipeline.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true if pipeline was removed successfully; otherwise, false.</returns>
        bool TryRemove(PipelineName name);

        /// <summary>
        /// Removes all pipelines.
        /// </summary>
        void Clear();

        /// <summary>
        /// Get pipeline by name or default.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        IPipeline<TContext>? GetOrDefault<TContext>(PipelineName name)
            where TContext : class;
    }
}
namespace PackSite.Library.Pipelining
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Pipeline collection.
    /// </summary>
    public interface IPipelineCollection : IReadOnlyCollection<IPipeline>
    {
        /// <summary>
        /// Event invoked when a pipeline was added to <see cref="IPipelineCollection"/>
        /// </summary>
        public event EventHandler<PipelineAddedEventArgs>? Added;

        /// <summary>
        /// Event invoked when a pipeline was updated in <see cref="IPipelineCollection"/>
        /// </summary>
        public event EventHandler<PipelineUpdatedEventArgs>? Updated;

        /// <summary>
        /// Event invoked when a pipeline was removed from <see cref="IPipelineCollection"/>
        /// </summary>
        public event EventHandler<PipelineRemovedEventArgs>? Removed;

        /// <summary>
        /// Event invoked when a pipeline collection was cleared.
        /// </summary>
        public event EventHandler? Cleared;

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
        /// Add or replace the specified pipeline.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <returns>true if pipeline was added, false if was updated</returns>
        bool AddOrUpdate(IPipeline pipeline);

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
        /// Get pipeline by name.
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when pipeline with specified name was not found.</exception>
        public IPipeline<TArgs> Get<TArgs>(PipelineName name)
            where TArgs : class;

        /// <summary>
        /// Get pipeline by name.
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when pipeline with specified name was not found.</exception>
        public IPipeline<TArgs> Get<TArgs>()
            where TArgs : class;

        /// <summary>
        /// Get pipeline by name. Returns null when not found.
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        IPipeline<TArgs>? GetOrDefault<TArgs>(PipelineName name)
            where TArgs : class;

        /// <summary>
        /// Get pipeline by its default name. Returns null when not found.
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <returns></returns>
        IPipeline<TArgs>? GetOrDefault<TArgs>()
            where TArgs : class;
    }
}
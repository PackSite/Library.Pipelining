namespace PackSite.Library.Pipelining
{
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Pipeline collection.
    /// </summary>
    public class PipelineCollection : IPipelineCollection
    {
        private readonly ConcurrentDictionary<PipelineName, IPipeline> _pipelines = new();

        /// <inheritdoc/>
        public IReadOnlyList<PipelineName> Names => _pipelines.Keys.ToList();

        /// <inheritdoc/>
        public int Count => _pipelines.Count;

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineCollection"/>.
        /// </summary>
        public PipelineCollection()
        {

        }

        /// <inheritdoc/>
        public bool TryAdd(IPipeline pipeline)
        {
            return _pipelines.TryAdd(pipeline.Name, pipeline);
        }

        /// <inheritdoc/>
        public bool TryRemove(PipelineName name)
        {
            return _pipelines.TryRemove(name, out IPipeline _);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _pipelines.Clear();
        }

        /// <inheritdoc/>
        public IPipeline<TContext>? GetOrDefault<TContext>(PipelineName name)
            where TContext : class
        {
            return _pipelines.GetValueOrDefault(name) as IPipeline<TContext>;
        }

        /// <inheritdoc/>
        public IEnumerator<IPipeline> GetEnumerator()
        {
            return _pipelines.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _pipelines.Values.GetEnumerator();
        }
    }
}

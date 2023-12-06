namespace PackSite.Library.Pipelining
{
    /// <summary>
    /// Pipeline collection.
    /// </summary>
    public class PipelineCollection : IPipelineCollection
    {
        private readonly ConcurrentDictionary<PipelineName, IPipeline> _pipelines = new();

        /// <inheritdoc/>
        public event EventHandler<PipelineAddedEventArgs>? Added;

        /// <inheritdoc/>
        public event EventHandler<PipelineUpdatedEventArgs>? Updated;

        /// <inheritdoc/>
        public event EventHandler<PipelineRemovedEventArgs>? Removed;

        /// <inheritdoc/>
        public event EventHandler? Cleared;

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
            bool wasAdded = _pipelines.TryAdd(pipeline.Name, pipeline);

            if (wasAdded && Added is not null)
            {
                Added(this, new PipelineAddedEventArgs(pipeline.Name));
            }

            return wasAdded;
        }

        /// <inheritdoc/>
        public bool AddOrUpdate(IPipeline pipeline)
        {
            PipelineName name = pipeline.Name;

            bool added = true;
            _pipelines.AddOrUpdate(
                name,
                pipeline,
                (keyToUpdate, oldPipeline) =>
                {
                    added = false;
                    return pipeline;
                });

            if (added)
            {
                Added?.Invoke(this, new PipelineAddedEventArgs(name));
            }
            else
            {
                Updated?.Invoke(this, new PipelineUpdatedEventArgs(name));
            }

            return added;
        }

        /// <inheritdoc/>
        public bool TryRemove(PipelineName name)
        {
            bool wasRemoved = _pipelines.TryRemove(name, out _);

            if (wasRemoved && Removed is not null)
            {
                Removed(this, new PipelineRemovedEventArgs(name));
            }

            return wasRemoved;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            _pipelines.Clear();
            Cleared?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public IPipeline<TArgs> Get<TArgs>(PipelineName name)
                where TArgs : class
        {
            return _pipelines.GetValueOrDefault(name) as IPipeline<TArgs> ??
                throw new ArgumentException($"Cannot get pipeline. Pipeline '{name}' not found or '{typeof(TArgs).FullName ?? typeof(TArgs).Name}' is an invalid type for '{name}' pipeline.", nameof(name));
        }

        /// <inheritdoc/>
        public IPipeline<TArgs> Get<TArgs>()
            where TArgs : class
        {
            return Get<TArgs>(IPipeline<TArgs>.DefaultName);
        }

        /// <inheritdoc/>
        public IPipeline<TArgs>? GetOrDefault<TArgs>(PipelineName name)
            where TArgs : class
        {
            return _pipelines.GetValueOrDefault(name) as IPipeline<TArgs>;
        }

        /// <inheritdoc/>
        public IPipeline<TArgs>? GetOrDefault<TArgs>()
            where TArgs : class
        {
            return GetOrDefault<TArgs>(IPipeline<TArgs>.DefaultName);
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

namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using PackSite.Library.Pipelining.StepActivators;

    /// <summary>
    /// Invokable pipeline factory implementation.
    /// </summary>
    internal sealed class InvokablePipelineFactory : IInvokablePipelineFactory, IDisposable
    {
        private readonly ConcurrentDictionary<PipelineName, Lazy<IInvokablePipeline>> _scopedPipelines = new();

        private readonly IPipelineCollection _pipelines;
        private readonly IScopedStepActivator _stepActivator;
        private readonly SingletonPipelines _singletonPipelines;

        /// <summary>
        /// Initializes a new instance of <see cref="InvokablePipelineFactory"/>.
        /// </summary>
        /// <param name="pipelines"></param>
        /// <param name="stepActivator"></param>
        /// <param name="singletonPipelines"></param>
        public InvokablePipelineFactory(IPipelineCollection pipelines,
                                        IScopedStepActivator stepActivator,
                                        SingletonPipelines singletonPipelines)
        {
            _pipelines = pipelines;
            _stepActivator = stepActivator;
            _singletonPipelines = singletonPipelines;

            pipelines.Cleared += PipelinesCollection_Cleared;
            pipelines.Updated += PipelinesCollection_Updated;
            pipelines.Removed += PipelinesCollection_Removed;
        }

        /// <inheritdoc/>
        public IInvokablePipeline<TArgs>? GetPipeline<TArgs>(PipelineName name)
            where TArgs : class
        {
            IPipeline<TArgs>? pipeline = _pipelines.GetOrDefault<TArgs>(name);

            if (pipeline is null)
            {
                return null;
            }

            if (pipeline.Lifetime is InvokablePipelineLifetime.Singleton)
            {
                return _singletonPipelines.Get(pipeline);
            }

            if (pipeline.Lifetime is InvokablePipelineLifetime.Scoped)
            {
                return _scopedPipelines.GetOrAdd(name, (key, p) =>
                {
                    return new Lazy<IInvokablePipeline>(() => p.CreateInvokable(_stepActivator), LazyThreadSafetyMode.ExecutionAndPublication);
                }, pipeline).Value as IInvokablePipeline<TArgs>;
            }

            return pipeline.CreateInvokable(_stepActivator);
        }

        /// <inheritdoc/>
        public IInvokablePipeline<TArgs> GetRequiredPipeline<TArgs>(PipelineName name)
            where TArgs : class
        {
            return GetPipeline<TArgs>(name) ??
                throw new ArgumentException($"Cannot get invokable pipeline. Pipeline '{name}' not found or '{typeof(TArgs).FullName ?? typeof(TArgs).Name}' is an invalid type for '{name}' pipeline.", nameof(name));
        }

        /// <inheritdoc/>
        public IInvokablePipeline<TArgs>? GetPipeline<TArgs>()
            where TArgs : class
        {
            return GetPipeline<TArgs>(IPipeline<TArgs>.DefaultName);
        }

        /// <inheritdoc/>
        public IInvokablePipeline<TArgs> GetRequiredPipeline<TArgs>()
            where TArgs : class
        {
            return GetRequiredPipeline<TArgs>(IPipeline<TArgs>.DefaultName);
        }

        private void PipelinesCollection_Cleared(object? sender, EventArgs e)
        {
            _scopedPipelines.Clear();
        }

        private void PipelinesCollection_Removed(object? sender, PipelineRemovedEventArgs e)
        {
            _scopedPipelines.TryRemove(e.PipelineName, out _);
        }

        private void PipelinesCollection_Updated(object? sender, PipelineUpdatedEventArgs e)
        {
            _scopedPipelines.TryRemove(e.PipelineName, out _);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _scopedPipelines.Clear();
            _pipelines.Cleared -= PipelinesCollection_Cleared;
            _pipelines.Updated -= PipelinesCollection_Updated;
            _pipelines.Removed -= PipelinesCollection_Removed;
        }

        /// <summary>
        /// Stores singleton pipelines.
        /// </summary>
        internal sealed class SingletonPipelines : IDisposable
        {
            private readonly IPipelineCollection _pipelines;
            private readonly ISingletonStepActivator _stepActivator;
            private readonly ConcurrentDictionary<PipelineName, IInvokablePipeline> _pipelinesCache = new();

            /// <summary>
            /// Initializes a new instance of <see cref="SingletonPipelines"/>.
            /// </summary>
            public SingletonPipelines(IPipelineCollection pipelines, ISingletonStepActivator stepActivator)
            {
                _pipelines = pipelines;
                _stepActivator = stepActivator;

                pipelines.Cleared += PipelinesCollection_Cleared;
                pipelines.Updated += PipelinesCollection_Updated;
                pipelines.Removed += PipelinesCollection_Removed;
            }

            /// <summary>
            /// Get singleton invokable pipeline.
            /// </summary>
            /// <param name="pipeline"></param>
            /// <returns></returns>
            public IInvokablePipeline<TArgs> Get<TArgs>(IPipeline<TArgs> pipeline)
                where TArgs : class
            {
                return (IInvokablePipeline<TArgs>)_pipelinesCache.GetOrAdd(pipeline.Name, (key, p) =>
                {
                    return pipeline.CreateInvokable(_stepActivator);
                }, pipeline);
            }

            private void PipelinesCollection_Cleared(object? sender, EventArgs e)
            {
                _pipelinesCache.Clear();
            }

            private void PipelinesCollection_Removed(object? sender, PipelineRemovedEventArgs e)
            {
                _pipelinesCache.TryRemove(e.PipelineName, out _);
            }

            private void PipelinesCollection_Updated(object? sender, PipelineUpdatedEventArgs e)
            {
                _pipelinesCache.TryRemove(e.PipelineName, out _);
            }

            /// <inheritdoc/>
            public void Dispose()
            {
                _pipelines.Cleared -= PipelinesCollection_Cleared;
                _pipelines.Updated -= PipelinesCollection_Updated;
                _pipelines.Removed -= PipelinesCollection_Removed;
            }
        }
    }
}

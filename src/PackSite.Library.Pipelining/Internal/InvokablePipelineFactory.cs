namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// Invokable pipeline factory implementation.
    /// </summary>
    internal sealed class InvokablePipelineFactory : IInvokablePipelineFactory, IDisposable
    {
        private readonly ConcurrentDictionary<PipelineName, object> _scopedPipelines = new();

        private readonly IPipelineCollection _pipelines;
        private readonly IStepActivator _stepActivator;
        private readonly SingletonPipelines _singletonPipelines;

        /// <summary>
        /// Initializes a new instance of <see cref="InvokablePipelineFactory"/>.
        /// </summary>
        /// <param name="pipelines"></param>
        /// <param name="stepActivator"></param>
        /// <param name="singletonPipelines"></param>
        public InvokablePipelineFactory(IPipelineCollection pipelines, IStepActivator stepActivator, SingletonPipelines singletonPipelines)
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
                return _singletonPipelines.Container.GetOrAdd(name, (key) =>
                {
                    return pipeline.CreateInvokable(_stepActivator);
                }) as IInvokablePipeline<TArgs>;
            }

            if (pipeline.Lifetime is InvokablePipelineLifetime.Scoped)
            {
                return _scopedPipelines.GetOrAdd(name, (key) =>
                {
                    return pipeline.CreateInvokable(_stepActivator);
                }) as IInvokablePipeline<TArgs>;
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
            return GetPipeline<TArgs>(typeof(IPipeline<TArgs>).FullName ?? string.Empty);
        }

        /// <inheritdoc/>
        public IInvokablePipeline<TArgs> GetRequiredPipeline<TArgs>()
            where TArgs : class
        {
            return GetRequiredPipeline<TArgs>(typeof(IPipeline<TArgs>).FullName ?? string.Empty);
        }

        private void PipelinesCollection_Cleared(object? sender, EventArgs e)
        {
            _scopedPipelines.Clear();
        }

        private void PipelinesCollection_Removed(object? sender, PipelineRemovedEventArgs e)
        {
            _scopedPipelines.TryRemove(e.PipelineName, out object _);
        }

        private void PipelinesCollection_Updated(object? sender, PipelineUpdatedEventArgs e)
        {
            _scopedPipelines.TryRemove(e.PipelineName, out object _);
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

            public ConcurrentDictionary<PipelineName, object> Container { get; } = new();

            /// <summary>
            /// Initializes a new instance of <see cref="SingletonPipelines"/>.
            /// </summary>
            public SingletonPipelines(IPipelineCollection pipelines)
            {
                _pipelines = pipelines;
                pipelines.Cleared += PipelinesCollection_Cleared;
                pipelines.Updated += PipelinesCollection_Updated;
                pipelines.Removed += PipelinesCollection_Removed;
            }

            private void PipelinesCollection_Cleared(object? sender, EventArgs e)
            {
                Container.Clear();
            }

            private void PipelinesCollection_Removed(object? sender, PipelineRemovedEventArgs e)
            {
                Container.TryRemove(e.PipelineName, out object _);
            }

            private void PipelinesCollection_Updated(object? sender, PipelineUpdatedEventArgs e)
            {
                Container.TryRemove(e.PipelineName, out object _);
            }

            public void Dispose()
            {
                Container.Clear();
                _pipelines.Cleared -= PipelinesCollection_Cleared;
                _pipelines.Updated -= PipelinesCollection_Updated;
                _pipelines.Removed -= PipelinesCollection_Removed;
            }
        }
    }
}

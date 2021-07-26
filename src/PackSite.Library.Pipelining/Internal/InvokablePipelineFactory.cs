namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// Invokable pipeline factory implementation.
    /// </summary>
    internal sealed class InvokablePipelineFactory : IInvokablePipelineFactory
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
        }

        /// <inheritdoc/>
        public IInvokablePipeline<TContext>? GetPipeline<TContext>(PipelineName name) where TContext : class
        {
            IPipeline<TContext>? pipeline = _pipelines.GetOrDefault<TContext>(name);

            if (pipeline is null)
            {
                return null;
            }

            if (pipeline.Lifetime is InvokablePipelineLifetime.Singleton)
            {
                return _singletonPipelines.Container.GetOrAdd(name, (key) =>
                {
                    return pipeline.CreateInvokable(_stepActivator);
                }) as IInvokablePipeline<TContext>;
            }

            if (pipeline.Lifetime is InvokablePipelineLifetime.Scoped)
            {
                return _scopedPipelines.GetOrAdd(name, (key) =>
                {
                    return pipeline.CreateInvokable(_stepActivator);
                }) as IInvokablePipeline<TContext>;
            }

            return pipeline.CreateInvokable(_stepActivator);
        }

        /// <inheritdoc/>
        public IInvokablePipeline<TContext> GetRequiredPipeline<TContext>(PipelineName name) where TContext : class
        {
            return GetPipeline<TContext>(name) ??
                throw new ArgumentException($"Cannot get invokable pipeline. Pipeline '{name}' not found or '{typeof(TContext).FullName ?? typeof(TContext).Name}' is an invalid type for '{name}' pipeline.", nameof(name));
        }

        /// <summary>
        /// Stores singleton pipelines.
        /// </summary>
        internal sealed class SingletonPipelines
        {
            public ConcurrentDictionary<PipelineName, object> Container { get; } = new();

            /// <summary>
            /// Initializes a new instance of <see cref="SingletonPipelines"/>.
            /// </summary>
            public SingletonPipelines()
            {

            }
        }
    }
}

namespace PackSite.Library.Pipelining.Internal
{
    using PackSite.Library.Pipelining;

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    internal sealed partial class PipelineBuilder<TArgs> : IPipelineBuilder<TArgs>
        where TArgs : class
    {
        private InvokablePipelineLifetime _lifetime = InvokablePipelineLifetime.Singleton;
        private PipelineName? _name;
        private string? _description;

        /// <inheritdoc/>
        public IStepCollection Steps { get; } = new StepCollection<TArgs>();

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineBuilder{TArgs}"/>.
        /// </summary>
        public PipelineBuilder()
        {

        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> Lifetime(InvokablePipelineLifetime lifetime)
        {
            _lifetime = lifetime;

            return this;
        }

        IPipelineBuilder IPipelineBuilder.Lifetime(InvokablePipelineLifetime lifetime)
        {
            return Lifetime(lifetime);
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> Name(PipelineName? name)
        {
            _name = name;

            return this;
        }

        IPipelineBuilder IPipelineBuilder.Name(PipelineName? name)
        {
            return Name(name);
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> Description(string description)
        {
            ArgumentNullException.ThrowIfNull(description);

            _description = description;

            return this;
        }

        IPipelineBuilder IPipelineBuilder.Description(string description)
        {
            return Description(description);
        }

        /// <inheritdoc/>
        public IPipeline<TArgs> Build()
        {
            _name ??= IPipeline<TArgs>.DefaultName;

            Pipeline<TArgs> pipeline = new(_lifetime,
                                           _name,
                                           _description ?? string.Empty,
                                           Steps.StepInstances,
                                           Steps.StepTypes);

            return pipeline;
        }

        IPipeline IPipelineBuilder.Build()
        {
            return Build();
        }
    }
}

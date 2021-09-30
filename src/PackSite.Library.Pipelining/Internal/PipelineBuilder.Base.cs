namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections.Generic;
    using PackSite.Library.Pipelining;

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    internal sealed partial class PipelineBuilder<TArgs> : IPipelineBuilder<TArgs>
        where TArgs : class
    {
        private readonly List<object> _steps = new();
        private readonly List<Type> _stepTypes = new();

        private InvokablePipelineLifetime _lifetime = InvokablePipelineLifetime.Singleton;
        private PipelineName? _name;
        private string? _description;

        /// <inheritdoc/>
        public int Count => _steps.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public Type this[int index]
        {
            get => _stepTypes[index];
            set => Insert(index, value);
        }

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
            _description = description ?? throw new ArgumentNullException(nameof(description));

            return this;
        }

        IPipelineBuilder IPipelineBuilder.Description(string description)
        {
            return Description(description);
        }

        /// <inheritdoc/>
        public IPipeline<TArgs> Build()
        {
            _name ??= typeof(IPipeline<TArgs>).FullName!;

            Pipeline<TArgs> pipeline = new(_lifetime,
                                           _name,
                                           _description ?? string.Empty,
                                           _steps,
                                           _stepTypes);

            return pipeline;
        }

        IPipeline IPipelineBuilder.Build()
        {
            return Build();
        }
    }
}

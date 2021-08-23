namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PackSite.Library.Pipelining;

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    internal sealed class PipelineBuilder<TArgs> : IPipelineBuilder<TArgs>
        where TArgs : class
    {
        private InvokablePipelineLifetime _lifetime = InvokablePipelineLifetime.Singleton;
        private readonly List<object> _buildTimeSteps = new();
        private PipelineName? _name;
        private string? _description;

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

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> Name(PipelineName? name)
        {
            _name = name;

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> Description(string description)
        {
            _description = description ?? throw new ArgumentNullException(nameof(description));

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> Step(Type stepType)
        {
            Type[] stepInterfaces = stepType.GetInterfaces();

            if (!stepInterfaces.Contains(typeof(IStep)) && !stepInterfaces.Contains(typeof(IStep<TArgs>)))
            {
                throw new ArgumentException(nameof(TArgs), $"Invalid step instance type. Must implement '{typeof(IStep).FullName}' or '{typeof(IStep<TArgs>).FullName}'.");
            }

            _buildTimeSteps.Add(stepType);

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> Step<TStep>()
            where TStep : class, IBaseStep
        {
            Type stepType = typeof(TStep);

            return Step(stepType);
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> Step<TStep>(TStep instance)
            where TStep : class, IBaseStep
        {
            _ = instance ?? throw new ArgumentNullException(nameof(instance));
            _ = _buildTimeSteps ?? throw new InvalidOperationException("Cannot modify pipeline after build operation.");

            if (instance is not (IStep or IStep<TArgs>))
            {
                throw new ArgumentException("Invalid step instance type.", nameof(instance));
            }

            _buildTimeSteps.Add(instance);

            return this;
        }

        /// <inheritdoc/>
        public IPipeline<TArgs> Build()
        {
            _name ??= typeof(IPipeline<TArgs>).FullName!;

            Pipeline<TArgs> pipeline = new(_lifetime,
                                              _name,
                                              _description ?? string.Empty,
                                              _buildTimeSteps.ToArray());

            return pipeline;
        }

        IPipelineBuilder IPipelineBuilder.Lifetime(InvokablePipelineLifetime lifetime)
        {
            return Lifetime(lifetime);
        }
        IPipelineBuilder IPipelineBuilder.Name(PipelineName? name)
        {
            return Name(name);
        }

        IPipelineBuilder IPipelineBuilder.Description(string description)
        {
            return Description(description);
        }

        IPipelineBuilder IPipelineBuilder.Step(Type stepType)
        {
            return Step(stepType);
        }

        IPipelineBuilder IPipelineBuilder.Step<TStep>()
        {
            return Step<TStep>();
        }

        IPipelineBuilder IPipelineBuilder.Step<TStep>(TStep instance)
        {
            return Step(instance);
        }

        IPipeline IPipelineBuilder.Build()
        {
            return Build();
        }
    }
}

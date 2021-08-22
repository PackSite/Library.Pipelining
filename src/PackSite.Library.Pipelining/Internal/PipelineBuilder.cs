namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PackSite.Library.Pipelining;

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    internal sealed class PipelineBuilder<TContext> : IPipelineBuilder<TContext>, IPipelineBuilder
        where TContext : class
    {
        private InvokablePipelineLifetime _lifetime = InvokablePipelineLifetime.Singleton;
        private readonly List<object> _buildTimeSteps = new();
        private PipelineName? _name;
        private string? _description;

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineBuilder{T}"/>.
        /// </summary>
        public PipelineBuilder()
        {

        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> Lifetime(InvokablePipelineLifetime lifetime)
        {
            _lifetime = lifetime;

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> Name(PipelineName? name)
        {
            _name = name;

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> Description(string description)
        {
            _description = description ?? throw new ArgumentNullException(nameof(description));

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> Add(Type stepType)
        {
            Type[] stepInterfaces = stepType.GetInterfaces();

            if (!stepInterfaces.Contains(typeof(IStep)) && !stepInterfaces.Contains(typeof(IStep<TContext>)))
            {
                throw new ArgumentException(nameof(TContext), $"Invalid step instance type. Must implement '{typeof(IStep).FullName}' or '{typeof(IStep<TContext>).FullName}'.");
            }

            _buildTimeSteps.Add(stepType);

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> Add<TStep>()
            where TStep : class, IBaseStep
        {
            Type stepType = typeof(TStep);

            return Add(stepType);
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> Add<TStep>(TStep instance)
            where TStep : class, IBaseStep
        {
            _ = instance ?? throw new ArgumentNullException(nameof(instance));
            _ = _buildTimeSteps ?? throw new InvalidOperationException("Cannot modify pipeline after build operation.");

            if (instance is not (IStep or IStep<TContext>))
            {
                throw new ArgumentException("Invalid step instance type.", nameof(instance));
            }

            _buildTimeSteps.Add(instance);

            return this;
        }

        /// <inheritdoc/>
        public IPipeline<TContext> Build()
        {
            _name ??= typeof(IPipeline<TContext>).FullName!;

            Pipeline<TContext> pipeline = new(_lifetime,
                                              _name,
                                              _description ?? string.Empty,
                                              _buildTimeSteps.ToArray());

            return pipeline;
        }

        IPipelineBuilder IPipelineBuilder.Lifetime(InvokablePipelineLifetime lifetime)
        {
            Lifetime(lifetime);
            return this;
        }
        IPipelineBuilder IPipelineBuilder.Name(PipelineName? name)
        {
            Name(name);
            return this;
        }

        IPipelineBuilder IPipelineBuilder.Description(string description)
        {
            Description(description);
            return this;
        }

        IPipelineBuilder IPipelineBuilder.Add(Type stepType)
        {
            Add(stepType);
            return this;
        }

        IPipelineBuilder IPipelineBuilder.Add<TStep>()
        {
            Add<TStep>();
            return this;
        }

        IPipelineBuilder IPipelineBuilder.Add<TStep>(TStep instance)
        {
            Add<TStep>(instance);
            return this;
        }

        IPipeline IPipelineBuilder.Build()
        {
            return Build();
        }
    }
}

namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections.Generic;
    using PackSite.Library.Pipelining;

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    internal sealed class PipelineBuilder<TContext> : IPipelineBuilder<TContext>
        where TContext : class
    {
        private InvokablePipelineLifetime _lifetime = InvokablePipelineLifetime.Scoped;
        private List<object>? _buildTimeSteps = new();
        private PipelineName _name;
        private string? _description;

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineBuilder{T}"/>.
        /// </summary>
        public PipelineBuilder()
        {
            _name = typeof(PipelineBuilder<TContext>).FullName!;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> Lifetime(InvokablePipelineLifetime lifetime)
        {
            _lifetime = lifetime;

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> Name(PipelineName name)
        {
            _ = _buildTimeSteps ?? throw new InvalidOperationException("Cannot modify pipeline after build operation.");
            _name = name;

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> Description(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException($"'{nameof(description)}' cannot be null or whitespace.", nameof(description));
            }

            _ = _buildTimeSteps ?? throw new InvalidOperationException("Cannot modify pipeline after build operation.");
            _description = description;

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> Add<TStep>()
            where TStep : class, IBaseStep
        {
            _ = _buildTimeSteps ?? throw new InvalidOperationException("Cannot modify pipeline after build operation.");
            _buildTimeSteps.Add(typeof(TStep));

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TContext> Add<TStep>(TStep instance)
            where TStep : class, IBaseStep
        {
            _ = instance ?? throw new ArgumentNullException(nameof(instance));
            _ = _buildTimeSteps ?? throw new InvalidOperationException("Cannot modify pipeline after build operation.");
            _buildTimeSteps.Add(instance);

            return this;
        }

        /// <inheritdoc/>
        public IPipeline<TContext> Build()
        {
            _ = _buildTimeSteps ?? throw new InvalidOperationException("Pipeline can be build only once.");

            Pipeline<TContext> pipeline = new Pipeline<TContext>(_lifetime,
                                                                     _name,
                                                                     _description,
                                                                     _buildTimeSteps.ToArray());

            _buildTimeSteps = null;

            return pipeline;
        }
    }
}

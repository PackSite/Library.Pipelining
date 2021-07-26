namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    internal sealed class Pipeline<TContext> : IPipeline<TContext>
        where TContext : class
    {
        private static readonly Func<TContext, CancellationToken, ValueTask> PipelineTermination = (input, cancellationToken) => default;

        private readonly object[] _steps;
        private readonly Lazy<IReadOnlyList<Type>> _lazyStepTypes;

        private string? _toStringCache;

        /// <inheritdoc/>
        public InvokablePipelineLifetime Lifetime { get; }

        /// <inheritdoc/>
        public PipelineName Name { get; }

        /// <inheritdoc/>
        public string? Description { get; }

        /// <inheritdoc/>
        public IReadOnlyList<Type> Steps => _lazyStepTypes.Value;

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineBuilder{T}"/>.
        /// </summary>
        /// <param name="lifetime"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="steps"></param>
        public Pipeline(InvokablePipelineLifetime lifetime, PipelineName name, string? description, object[] steps)
        {
            Lifetime = lifetime;
            Name = name;
            Description = description;
            _steps = steps;

            _lazyStepTypes = new Lazy<IReadOnlyList<Type>>(() =>
            {
                List<Type> types = new(_steps.Length);
                foreach (object step in _steps)
                {
                    Type stepType = step is Type s ? s : step.GetType();

                    types.Add(stepType);
                }

                return types;
            });
        }

        /// <inheritdoc/>
        public IInvokablePipeline<TContext> CreateInvokable(IStepActivator stepActivator)
        {
            IBaseStep?[] instances = new IBaseStep?[_steps.Length];

            for (int i = 0; i < _steps.Length; i++)
            {
                object step = _steps[i];

                if (step is Type stepType)
                {
                    instances[i] = stepActivator.Create(stepType);
                }
                else if (step is IBaseStep baseStep)
                {
                    instances[i] = baseStep;
                }
            }

            Func<TContext, CancellationToken, ValueTask> invokeDelegate = PipelineTermination;
            for (int i = _steps.Length - 1; i >= 0; i--)
            {
                IBaseStep? baseStep = instances[i];
                Func<TContext, CancellationToken, ValueTask> next = invokeDelegate;

                if (baseStep is IStep s)
                {
                    invokeDelegate = (input, ct) =>
                    {
                        return s.ExecuteAsync(() => next(input, ct), ct);
                    };
                }
                else if (baseStep is IStep<TContext> sp)
                {
                    invokeDelegate = (input, ct) =>
                    {
                        return sp.ExecuteAsync(input, () => next(input, ct), ct);
                    };
                }
            }

            return new InvokablePipeline<TContext>(this, invokeDelegate);
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            if (_toStringCache is not null)
            {
                return _toStringCache;
            }

            StringBuilder builder = new();

            builder.Append("PIPELINE '");
            builder.Append(Name);
            builder.AppendLine("'");
            builder.AppendLine("{");

            int i = 0;

            foreach (Type step in Steps)
            {
                builder.Append("  [");
                builder.Append(i++);
                builder.Append("] = ");
                builder.AppendLine(step.FullName ?? step.Name);
            }

            builder.AppendLine("  [-] = \\/ /\\");

            foreach (Type step in Steps.Reverse())
            {
                builder.Append("  [");
                builder.Append(i--);
                builder.Append("] = ");
                builder.AppendLine(step.FullName ?? step.Name);
            }

            builder.AppendLine("}");
            _toStringCache = builder.ToString();

            return _toStringCache;
        }
    }
}

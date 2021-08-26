namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining.Internal.Extensions;

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    internal sealed class Pipeline<TArgs> : IPipeline<TArgs>
        where TArgs : class
    {
        /// <summary>
        /// Step delegate.
        /// </summary>
        /// <returns></returns>
        internal delegate ValueTask ConcreteStepDelegate(TArgs args,
                                                         IInvokablePipeline invokablePipeline,
                                                         StepDelegate terminationContinuation,
                                                         CancellationToken cancellationToken);

        private readonly PipelineCounters _counters = new();

        private readonly IReadOnlyList<object> _steps;
        private readonly Lazy<IReadOnlyList<Type>> _lazyStepTypes;

        private string? _toStringCache;

        /// <inheritdoc/>
        public IPipelineCounters Counters => _counters;

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
        public Pipeline(InvokablePipelineLifetime lifetime, PipelineName name, string description, IReadOnlyList<object> steps)
        {
            Lifetime = lifetime;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            _steps = steps ?? throw new ArgumentNullException(nameof(steps));

            _lazyStepTypes = new Lazy<IReadOnlyList<Type>>(() =>
            {
                List<Type> types = new(_steps.Count);
                foreach (object step in _steps)
                {
                    Type stepType = step is Type s ? s : step.GetType();

                    types.Add(stepType);
                }

                return types;
            });
        }

        /// <inheritdoc/>
        public IInvokablePipeline<TArgs> CreateInvokable(IStepActivator stepActivator)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            IBaseStep?[] instances = new IBaseStep?[_steps.Count];

            for (int i = 0; i < _steps.Count; i++)
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

            //TODO: pipeline step profiling

            ConcreteStepDelegate? invokeDelegate = null;
            for (int i = _steps.Count - 1; i >= 0; i--)
            {
                IBaseStep? baseStep = instances[i];
                ConcreteStepDelegate? next = invokeDelegate;

                if (next is not null)
                {
                    if (baseStep is IStep s)
                    {
                        invokeDelegate = (input, invokablePipeline, terminationContinuation, ct) =>
                        {
                            return s.ExecuteAsync(
                                input,
                                () => next(input, invokablePipeline, terminationContinuation, ct),
                                invokablePipeline,
                                ct);
                        };
                    }
                    else if (baseStep is IStep<TArgs> sp)
                    {
                        invokeDelegate = (input, invokablePipeline, terminationContinuation, ct) =>
                        {
                            return sp.ExecuteAsync(
                                input,
                                () => next(input, invokablePipeline, terminationContinuation, ct),
                                (invokablePipeline as IInvokablePipeline<TArgs>)!,
                                ct);
                        };
                    }
                }
                else
                {
                    if (baseStep is IStep s)
                    {
                        invokeDelegate = (input, invokablePipeline, terminationContinuation, ct) =>
                        {
                            return s.ExecuteAsync(
                                input,
                                terminationContinuation,
                                invokablePipeline,
                                ct);
                        };
                    }
                    else if (baseStep is IStep<TArgs> sp)
                    {
                        invokeDelegate = (input, invokablePipeline, terminationContinuation, ct) =>
                        {
                            return sp.ExecuteAsync(
                                input,
                                terminationContinuation,
                                (invokablePipeline as IInvokablePipeline<TArgs>)!,
                                ct);
                        };
                    }
                }
            }

            InvokablePipeline<TArgs> invokablePipeline = new(this, _counters, invokeDelegate);

            stopwatch.Stop();
            _counters.ReportBuilt(stopwatch.ElapsedMicroseconds());

            return invokablePipeline;
        }

        IInvokablePipeline IPipeline.CreateInvokable(IStepActivator stepActivator)
        {
            return CreateInvokable(stepActivator);
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
            builder.Append(" (");
            builder.Append(Steps.Count);

            if (Steps.Count == 1)
            {
                builder.Append(" step)");
            }
            else
            {
                builder.Append(" steps)");
            }

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
            --i;

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

namespace PackSite.Library.Pipelining.Internal
{
    using System.Diagnostics;
    using PackSite.Library.Pipelining.Internal.Extensions;

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    internal sealed class Pipeline<TArgs> : IPipeline<TArgs>
        where TArgs : class
    {
        private readonly PipelineCounters _counters = new();

        private readonly IReadOnlyList<object> _steps;
        private readonly IReadOnlyList<Type> _stepTypes;

        private string? _toFullNameStringCache;
        private string? _toStepsStringCache;

        /// <inheritdoc/>
        public IPipelineCounters Counters => _counters;

        /// <inheritdoc/>
        public InvokablePipelineLifetime Lifetime { get; }

        /// <inheritdoc/>
        public PipelineName Name { get; }

        /// <inheritdoc/>
        public string Description { get; }

        /// <inheritdoc/>
        IReadOnlyList<Type> IPipeline.Steps => _stepTypes;

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineBuilder{T}"/>.
        /// </summary>
        /// <param name="lifetime"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="steps"></param>
        /// <param name="stepTypes"></param>
        public Pipeline(InvokablePipelineLifetime lifetime,
                        PipelineName name,
                        string description,
                        IReadOnlyList<object> steps,
                        IReadOnlyList<Type> stepTypes)
        {
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(description);
            ArgumentNullException.ThrowIfNull(steps);
            ArgumentNullException.ThrowIfNull(stepTypes);

            Lifetime = lifetime;
            Name = name;
            Description = description;
            _steps = steps;
            _stepTypes = stepTypes;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> CreateBuilder()
        {
            IPipelineBuilder<TArgs> builder = PipelineBuilder.Create<TArgs>()
                .Name(Name)
                .Description(Description)
                .Lifetime(Lifetime);

            foreach (object step in _steps)
            {
                if (step is Type s)
                {
                    builder.Add(s);
                }
                else if (step is IBaseStep baseStep)
                {
                    builder.Add(baseStep);
                }
            }

            return builder;
        }

        /// <inheritdoc/>
        public IInvokablePipeline<TArgs> CreateInvokable(IBaseStepActivator stepActivator)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            IReadOnlyList<object> tmp = _steps;

            IStep?[] universalSteps = new IStep?[tmp.Count];
            IStep<TArgs>?[] genericSteps = new IStep<TArgs>?[tmp.Count];

            for (int i = 0; i < tmp.Count; i++)
            {
                switch (tmp[i])
                {
                    case Type stepType:
                        {
                            IBaseStep s = stepActivator.Create(stepType);

                            if (s is IStep<TArgs> gs)
                            {
                                genericSteps[i] = gs;
                            }
                            else if (s is IStep us)
                            {
                                universalSteps[i] = us;
                            }

                            break;
                        }

                    case IStep<TArgs> gs:
                        genericSteps[i] = gs;
                        break;

                    case IStep us:
                        universalSteps[i] = us;
                        break;
                }
            }

            InvokablePipeline<TArgs> invokablePipeline = new(this, _counters, universalSteps, genericSteps);

            stopwatch.Stop();
            _counters.ReportBuilt(stopwatch.ElapsedMicroseconds());

            return invokablePipeline;
        }

        IInvokablePipeline IPipeline.CreateInvokable(IBaseStepActivator stepActivator)
        {
            return CreateInvokable(stepActivator);
        }

        IPipelineBuilder IPipeline.CreateBuilder()
        {
            return CreateBuilder();
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return ToString(null, null);
        }

        /// <inheritdoc/>
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                format = "f";
            }

            formatProvider ??= CultureInfo.CurrentCulture;

            return format.ToLowerInvariant() switch
            {
                "n" or "name" or "d" or "default" => Name,
                "f" or "full" or "fullname" => ToFullNameString(),
                "s" or "steps" => ToStepsString(formatProvider),

                _ => throw new FormatException($"The {format} format string is not supported."),
            };
        }

        private string ToFullNameString()
        {
            _toFullNameStringCache ??= $"{Name}, {IPipeline<TArgs>.DefaultName}";

            return _toFullNameStringCache;
        }

        private string ToStepsString(IFormatProvider formatProvider)
        {
            if (_toStepsStringCache is not null)
            {
                return _toStepsStringCache;
            }

            StringBuilder builder = new();

            builder.Append(ToString("f", null));
            builder.Append(" (");
            builder.Append(_stepTypes.Count);

            if (_stepTypes.Count == 1)
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

            foreach (Type step in _stepTypes)
            {
                builder.Append("  [");
                builder.Append(i++);
                builder.Append("] = ");
                builder.AppendLine(step.FullName?.ToString(formatProvider));
            }

            builder.AppendLine("  [-] = \\/ /\\");
            --i;

            foreach (Type step in _stepTypes.Reverse())
            {
                builder.Append("  [");
                builder.Append(i--);
                builder.Append("] = ");
                builder.AppendLine(step.FullName?.ToString(formatProvider));
            }

            builder.AppendLine("}");
            _toStepsStringCache = builder.ToString();

            return _toStepsStringCache;
        }
    }
}

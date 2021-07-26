namespace PackSite.Library.Pipelining
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Pipeline builder helper.
    /// </summary>
    public static class PipelineBuilder
    {
        /// <summary>
        /// Creates a new instance of pipeline builder.
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <returns></returns>
        public static IPipelineBuilder<TParameter> Create<TParameter>()
            where TParameter : class
        {
            return new PipelineBuilder<TParameter>(); //TODO: maybe rename TParameter to TContext
        }
    }

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    internal sealed class PipelineBuilder<TParameter> : IPipelineBuilder<TParameter>, IPipeline<TParameter>
        where TParameter : class
    {
        private static readonly Func<TParameter, CancellationToken, ValueTask> PipelineTermination = (input, cancellationToken) => default;

        private List<object>? _buildTimeSteps = new();
        private string _name;
        private string? _description;
        private object[] _steps = Array.Empty<object>();

        private string? _toStringCache;

        string IPipeline.Name => _name;
        string? IPipeline.Description => _description;

        /// <summary>
        /// Initializes a new instance of <see cref="PipelineBuilder{T}"/>.
        /// </summary>
        public PipelineBuilder()
        {
            _name = typeof(PipelineBuilder<TParameter>).FullName!;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TParameter> Name(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            _ = _buildTimeSteps ?? throw new InvalidOperationException("Cannot modify pipeline after build operation.");
            _name = name;

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TParameter> Description(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException($"'{nameof(description)}' cannot be null or whitespace.", nameof(description));
            }

            _ = _buildTimeSteps ?? throw new InvalidOperationException("Cannot modify pipeline after build operation.");
            _description = description; ;

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TParameter> Add<TStep>()
            where TStep : class, IBaseStep
        {
            _ = _buildTimeSteps ?? throw new InvalidOperationException("Cannot modify pipeline after build operation.");
            _buildTimeSteps.Add(typeof(TStep));

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TParameter> Add<TStep>(TStep instance)
            where TStep : class, IBaseStep
        {
            _ = _buildTimeSteps ?? throw new InvalidOperationException("Cannot modify pipeline after build operation.");
            _buildTimeSteps.Add(instance);

            return this;
        }

        /// <inheritdoc/>
        public IPipeline<TParameter> Build()
        {
            _ = _buildTimeSteps ?? throw new InvalidOperationException("Pipeline can be build only once.");

            _steps = _buildTimeSteps.ToArray();
            _buildTimeSteps = null;

            return this;
        }

        IInvokablePipeline<TParameter> IPipeline<TParameter>.AsInvokable(IStepActivator stepActivator)
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

            Func<TParameter, CancellationToken, ValueTask> invokeDelegate = PipelineTermination;
            for (int i = _steps.Length - 1; i >= 0; i--)
            {
                IBaseStep? baseStep = instances[i];
                Func<TParameter, CancellationToken, ValueTask> next = invokeDelegate;

                if (baseStep is IStep s)
                {
                    invokeDelegate = (input, ct) =>
                    {
                        return s.ExecuteAsync(() => next(input, ct), ct);
                    };
                }
                else if (baseStep is IStep<TParameter> sp)
                {
                    invokeDelegate = (input, ct) =>
                    {
                        return sp.ExecuteAsync(input, () => next(input, ct), ct);
                    };
                }
            }

            return new InvokablePipeline<TParameter>(this, invokeDelegate);
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            if (_buildTimeSteps is null && _toStringCache is not null)
            {
                return _toStringCache;
            }

            IEnumerable<object> steps = (IEnumerable<object>?)_buildTimeSteps ?? _steps;
            StringBuilder builder = new();

            builder.Append("PIPELINE '");
            builder.Append(_name);
            builder.AppendLine("'");
            builder.AppendLine("{");

            int i = 0;

            foreach (object step in steps)
            {
                Type stepType = step is Type s ? s : step.GetType();

                builder.Append("  [");
                builder.Append(i++);
                builder.Append("] = ");
                builder.AppendLine(stepType.FullName ?? stepType.Name);
            }

            builder.AppendLine("  [-] = \\/ /\\");

            foreach (object step in steps.Reverse())
            {
                Type stepType = step is Type s ? s : step.GetType();

                builder.Append("  [");
                builder.Append(i--);
                builder.Append("] = ");
                builder.AppendLine(stepType.FullName ?? stepType.Name);
            }

            builder.AppendLine("}");
            string v = builder.ToString();
            if (_buildTimeSteps is null)
            {
                _toStringCache = v;
            }

            return v;
        }

        private string GetDebuggerDisplay()
        {
            return $"'{_buildTimeSteps?.Count ?? _steps.Count()}' step pipeline '{_name}'";
        }
    }
}

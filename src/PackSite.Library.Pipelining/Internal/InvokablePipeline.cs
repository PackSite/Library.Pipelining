namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Internal.Extensions;

    /// <summary>
    /// Invokable pipleline.
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    internal sealed class InvokablePipeline<TArgs> : IInvokablePipeline<TArgs>
        where TArgs : class
    {
        /// <summary>
        /// Invokable pipeline termination.
        /// </summary>
        internal static readonly StepDelegate Termination = () => default;

        private readonly PipelineCounters _pipelineCounters;
        private readonly InvokablePipelineCounters _invokablePipelineCounters = new();
        private readonly Pipeline<TArgs>.ConcreteStepDelegate? _delegate;

        /// <inheritdoc/>
        public IInvokablePipelineCounters Counters => _invokablePipelineCounters;

        /// <inheritdoc/>
        public IPipeline<TArgs> Pipeline { get; }
        IPipeline IInvokablePipeline.Pipeline => Pipeline;

        /// <summary>
        /// Initializes a new instance of <see cref="InvokablePipeline{TArgs}"/>.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <param name="pipelineCounters"></param>
        /// <param name="delegate"></param>
        public InvokablePipeline(IPipeline<TArgs> pipeline, PipelineCounters pipelineCounters, Pipeline<TArgs>.ConcreteStepDelegate? @delegate)
        {
            Pipeline = pipeline;
            _pipelineCounters = pipelineCounters;
            _delegate = @delegate;
        }

        /// <inheritdoc/>
        public ValueTask<TArgs> InvokeAsync(TArgs input, CancellationToken cancellationToken = default)
        {
            if (_delegate is null)
            {
                _invokablePipelineCounters.Success(0);
                _pipelineCounters.Success(0);

                return new ValueTask<TArgs>(input);
            }

            return InvokeAsync(input, Termination, cancellationToken);
        }

        /// <inheritdoc/>
        public async ValueTask<TArgs> InvokeAsync(TArgs input, StepDelegate terminationContinuation, CancellationToken cancellationToken = default)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                if (_delegate is null)
                {
                    await terminationContinuation();
                }
                else
                {
                    await _delegate(input, this, terminationContinuation, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                long elapsedUsForFailed = stopwatch.ElapsedMicroseconds();
                _invokablePipelineCounters.Fail(elapsedUsForFailed);
                _pipelineCounters.Fail(elapsedUsForFailed);

                throw new PipelineInvocationException(input, Pipeline, ex);
            }

            stopwatch.Stop();
            long elapsedUs = stopwatch.ElapsedMicroseconds();
            _invokablePipelineCounters.Success(elapsedUs);
            _pipelineCounters.Success(elapsedUs);

            return input;
        }

        /// <inheritdoc/>
        public async ValueTask<object> InvokeAsync(object args, CancellationToken cancellationToken = default)
        {
            return await InvokeAsync((TArgs)args, cancellationToken);
        }

        public async ValueTask<object> InvokeAsync(object args, StepDelegate terminationContinuation, CancellationToken cancellationToken = default)
        {
            return await InvokeAsync((TArgs)args, terminationContinuation, cancellationToken);
        }
    }
}

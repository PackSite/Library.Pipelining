namespace PackSite.Library.Pipelining.Internal
{
    using System.Diagnostics;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Internal.Extensions;

    /// <summary>
    /// Invokable pipeline.
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    /// <remarks>
    /// Initializes a new instance of <see cref="InvokablePipeline{TArgs}"/>.
    /// </remarks>
    /// <param name="pipeline"></param>
    /// <param name="pipelineCounters"></param>
    /// <param name="universalSteps"></param>
    /// <param name="genericSteps"></param>
    internal sealed class InvokablePipeline<TArgs>(
        IPipeline<TArgs> pipeline,
        PipelineCounters pipelineCounters,
        IStep?[] universalSteps,
        IStep<TArgs>?[] genericSteps) : IInvokablePipeline<TArgs>
        where TArgs : class
    {
        /// <summary>
        /// Step delegate.
        /// </summary>
        /// <returns></returns>
        internal delegate ValueTask ConcreteStepDelegate(IBaseStep[] steps,
                                                         ref int index,
                                                         TArgs args,
                                                         StepDelegate terminationContinuation,
                                                         CancellationToken cancellationToken);

        private readonly PipelineCounters _pipelineCounters = pipelineCounters;
        private readonly InvokablePipelineCounters _invokablePipelineCounters = new();
        private readonly IStep?[] _universalSteps = universalSteps;
        private readonly IStep<TArgs>?[] _genericSteps = genericSteps;
        private readonly int _stepsCount = Math.Max(universalSteps.Length, genericSteps.Length);

        /// <inheritdoc/>
        public IInvokablePipelineCounters Counters => _invokablePipelineCounters;

        /// <inheritdoc/>
        public IPipeline<TArgs> Pipeline { get; } = pipeline;
        IPipeline IInvokablePipeline.Pipeline => Pipeline;

        /// <inheritdoc/>
        public Task<TArgs> InvokeAsync(TArgs input, CancellationToken cancellationToken = default)
        {
            if (_universalSteps.Length == 0)
            {
                _invokablePipelineCounters.Success(0);
                _pipelineCounters.Success(0);

                return Task.FromResult(input);
            }

            return InvokeAsync(input, null, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<TArgs> InvokeAsync(TArgs input, StepDelegate? terminationContinuation, CancellationToken cancellationToken = default)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                await StepAsync(0, input, terminationContinuation, cancellationToken);
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

        private async Task StepAsync(int index,
                                     TArgs args,
                                     StepDelegate? terminationContinuation,
                                     CancellationToken cancellationToken)
        {
            if (index >= _stepsCount)
            {
                if (terminationContinuation is not null)
                {
                    await terminationContinuation();
                }

                return;
            }

            cancellationToken.ThrowIfCancellationRequested();

            Task Next()
            {
                return StepAsync(index + 1, args, terminationContinuation, cancellationToken);
            }

            IStep<TArgs>? genericStep = _genericSteps[index];
            if (genericStep is not null)
            {
                await genericStep.ExecuteAsync(args, Next, this, cancellationToken);
            }
            else
            {
                IStep? universalStep = _universalSteps[index];
                if (universalStep is not null)
                {
                    await universalStep.ExecuteAsync(args, Next, this, cancellationToken);
                }
            }
        }

        /// <inheritdoc/>
        public async Task<object> InvokeAsync(object args, CancellationToken cancellationToken = default)
        {
            return await InvokeAsync((TArgs)args, cancellationToken);
        }

        public async Task<object> InvokeAsync(object args, StepDelegate? terminationContinuation, CancellationToken cancellationToken = default)
        {
            return await InvokeAsync((TArgs)args, terminationContinuation, cancellationToken);
        }
    }
}

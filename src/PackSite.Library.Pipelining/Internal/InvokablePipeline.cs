namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Internal.Extensions;

    /// <summary>
    /// Invokable pipeline.
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    internal sealed class InvokablePipeline<TArgs> : IInvokablePipeline<TArgs>
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

        private readonly PipelineCounters _pipelineCounters;
        private readonly InvokablePipelineCounters _invokablePipelineCounters = new();
        private readonly IStep?[] _universalSteps;
        private readonly IStep<TArgs>?[] _genericSteps;
        private readonly int _stepsCount;

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
        /// <param name="universalSteps"></param>
        /// <param name="genericSteps"></param>
        public InvokablePipeline(IPipeline<TArgs> pipeline,
                                 PipelineCounters pipelineCounters,
                                 IStep?[] universalSteps,
                                 IStep<TArgs>?[] genericSteps)
        {
            Pipeline = pipeline;
            _pipelineCounters = pipelineCounters;
            _universalSteps = universalSteps;
            _genericSteps = genericSteps;

            _stepsCount = Math.Max(universalSteps.Length, genericSteps.Length);
        }

        /// <inheritdoc/>
        public ValueTask<TArgs> InvokeAsync(TArgs input, CancellationToken cancellationToken = default)
        {
            if (_universalSteps.Length <= 0)
            {
                _invokablePipelineCounters.Success(0);
                _pipelineCounters.Success(0);

                return new ValueTask<TArgs>(input);
            }

            return InvokeAsync(input, null, cancellationToken);
        }

        /// <inheritdoc/>
        public async ValueTask<TArgs> InvokeAsync(TArgs input, StepDelegate? terminationContinuation, CancellationToken cancellationToken = default)
        {
            //TODO: pipeline step profiling

            var stopwatch = Stopwatch.StartNew();

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

        private async ValueTask StepAsync(int index,
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

            ValueTask Next()
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
        public async ValueTask<object> InvokeAsync(object args, CancellationToken cancellationToken = default)
        {
            return await InvokeAsync((TArgs)args, cancellationToken);
        }

        public async ValueTask<object> InvokeAsync(object args, StepDelegate? terminationContinuation, CancellationToken cancellationToken = default)
        {
            return await InvokeAsync((TArgs)args, terminationContinuation, cancellationToken);
        }
    }
}

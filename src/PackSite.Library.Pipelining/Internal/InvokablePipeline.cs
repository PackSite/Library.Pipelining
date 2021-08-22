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
        private readonly PipelineCounters _pipelineCounters;
        private readonly PipelineCounters _invokablePipelineCounters = new();
        private readonly Func<TArgs, IInvokablePipeline, CancellationToken, ValueTask> _delegate;

        /// <inheritdoc/>
        public IPipelineCounters Counters => _invokablePipelineCounters;

        /// <inheritdoc/>
        public IPipeline<TArgs> Pipeline { get; }
        IPipeline IInvokablePipeline.Pipeline => Pipeline;

        /// <summary>
        /// Initializes a new instance of <see cref="InvokablePipeline{TArgs}"/>.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <param name="pipelineCounters"></param>
        /// <param name="delegate"></param>
        public InvokablePipeline(IPipeline<TArgs> pipeline, PipelineCounters pipelineCounters, Func<TArgs, IInvokablePipeline, CancellationToken, ValueTask> @delegate)
        {
            Pipeline = pipeline;
            _pipelineCounters = pipelineCounters;
            _delegate = @delegate;
        }

        /// <inheritdoc/>
        public async ValueTask<TArgs> InvokeAsync(TArgs input, CancellationToken cancellationToken = default)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            try
            {
                await _delegate(input, this, cancellationToken);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _invokablePipelineCounters.Fail(stopwatch.ElapsedMicroseconds());
                _pipelineCounters.Fail(stopwatch.ElapsedMicroseconds());
                throw new PipelineInvocationException(input, Pipeline, ex);
            }

            stopwatch.Stop();
            _invokablePipelineCounters.Success(stopwatch.ElapsedMicroseconds());
            _pipelineCounters.Success(stopwatch.ElapsedMicroseconds());

            return input;
        }

        /// <inheritdoc/>
        public async ValueTask<object> InvokeAsync(object args, CancellationToken cancellationToken = default)
        {
            return await InvokeAsync((TArgs)args, cancellationToken);
        }
    }
}

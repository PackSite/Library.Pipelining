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
    /// <typeparam name="TContext"></typeparam>
    internal sealed class InvokablePipeline<TContext> : IInvokablePipeline<TContext>
        where TContext : class
    {
        private readonly PipelineCounters _pipelineCounters;
        private readonly PipelineCounters _invokablePipelineCounters = new();
        private readonly Func<TContext, IInvokablePipeline, CancellationToken, ValueTask> _delegate;

        /// <inheritdoc/>
        public IPipelineCounters Counters => _invokablePipelineCounters;

        /// <inheritdoc/>
        public IPipeline<TContext> Pipeline { get; }
        IPipeline IInvokablePipeline.Pipeline => Pipeline;

        /// <summary>
        /// Initializes a new instance of <see cref="InvokablePipeline{TContext}"/>.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <param name="pipelineCounters"></param>
        /// <param name="delegate"></param>
        public InvokablePipeline(IPipeline<TContext> pipeline, PipelineCounters pipelineCounters, Func<TContext, IInvokablePipeline, CancellationToken, ValueTask> @delegate)
        {
            Pipeline = pipeline;
            _pipelineCounters = pipelineCounters;
            _delegate = @delegate;
        }

        /// <inheritdoc/>
        public async ValueTask<TContext> InvokeAsync(TContext input, CancellationToken cancellationToken = default)
        {
            //TODO: pipeline profiling
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
        public async ValueTask<object> InvokeAsync(object context, CancellationToken cancellationToken = default)
        {
            return await InvokeAsync((TContext)context, cancellationToken);
        }
    }
}

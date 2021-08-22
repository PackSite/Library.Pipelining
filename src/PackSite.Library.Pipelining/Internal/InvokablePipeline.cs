namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining;

    /// <summary>
    /// Invokable pipleline.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    internal sealed class InvokablePipeline<TContext> : IInvokablePipeline<TContext>
        where TContext : class
    {
        private readonly Func<TContext, CancellationToken, ValueTask> _delegate;

        /// <inheritdoc/>
        public IPipeline<TContext> Pipeline { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="InvokablePipeline{TContext}"/>.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <param name="delegate"></param>
        public InvokablePipeline(IPipeline<TContext> pipeline, Func<TContext, CancellationToken, ValueTask> @delegate)
        {
            Pipeline = pipeline;
            _delegate = @delegate;
        }

        /// <inheritdoc/>
        public async ValueTask<TContext> InvokeAsync(TContext input, CancellationToken cancellationToken = default)
        {
            //TODO: pipeline profiling
            try
            {
                await _delegate(input, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new PipelineInvocationException(input, Pipeline, ex);
            }

            return input;
        }
    }
}

namespace PackSite.Library.Pipelining
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Invokable pipleline.
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    internal sealed class InvokablePipeline<TParameter> : IInvokablePipeline<TParameter>
        where TParameter : class
    {
        private readonly Func<TParameter, CancellationToken, ValueTask> _delegate;

        /// <inheritdoc/>
        public IPipeline<TParameter> Pipeline { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="InvokablePipeline{TParameter}"/>.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <param name="delegate"></param>
        public InvokablePipeline(IPipeline<TParameter> pipeline, Func<TParameter, CancellationToken, ValueTask> @delegate)
        {
            Pipeline = pipeline;
            _delegate = @delegate;
        }

        /// <inheritdoc/>
        public async ValueTask<TParameter> InvokeAsync(TParameter input, CancellationToken cancellationToken = default)
        {
            try
            {
                await _delegate(input, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new PipelineException(Pipeline, ex);
            }

            return input;
        }
    }
}

namespace PackSite.Library.Pipelining
{
    using System.Threading;
    using System.Threading.Tasks;
    using PackSite.Library.Pipelining.Exceptions;

    /// <summary>
    /// Invokable pipeline.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IInvokablePipeline<TContext>
        where TContext : class
    {
        /// <summary>
        /// Pipeline.
        /// </summary>
        IPipeline<TContext> Pipeline { get; }

        /// <summary>
        /// Invokes a pipeline for given input.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="PipelineException">Throws when an unhandled exception was thrown during pipeline execution.</exception>
        ValueTask<TContext> InvokeAsync(TContext input, CancellationToken cancellationToken = default);
    }
}
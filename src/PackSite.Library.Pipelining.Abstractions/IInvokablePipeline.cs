namespace PackSite.Library.Pipelining
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Invokable pipeline.
    /// </summary>
    public interface IInvokablePipeline
    {
        /// <summary>
        /// Invokable pipeline instance counters.
        /// </summary>
        IPipelineCounters Counters { get; }

        /// <summary>
        /// Pipeline.
        /// </summary>
        IPipeline Pipeline { get; }

        /// <summary>
        /// Invokes a pipeline for given input.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="PipelineInvocationException">Throws when an unhandled exception was thrown during pipeline execution.</exception>
        /// <exception cref="InvalidCastException">Throws when failed to cast <paramref name="context"/> to underlying context type.</exception>
        ValueTask<object> InvokeAsync(object context, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Invokable pipeline.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IInvokablePipeline<TContext> : IInvokablePipeline
        where TContext : class
    {
        /// <summary>
        /// Pipeline.
        /// </summary>
        new IPipeline<TContext> Pipeline { get; }

        /// <summary>
        /// Invokes a pipeline for given input.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="PipelineInvocationException">Throws when an unhandled exception was thrown during pipeline execution.</exception>
        ValueTask<TContext> InvokeAsync(TContext context, CancellationToken cancellationToken = default);
    }
}
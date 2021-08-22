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
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="PipelineInvocationException">Throws when an unhandled exception was thrown during pipeline execution.</exception>
        /// <exception cref="InvalidCastException">Throws when failed to cast <paramref name="args"/> to underlying args type.</exception>
        ValueTask<object> InvokeAsync(object args, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Invokable pipeline.
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    public interface IInvokablePipeline<TArgs> : IInvokablePipeline
        where TArgs : class
    {
        /// <summary>
        /// Pipeline.
        /// </summary>
        new IPipeline<TArgs> Pipeline { get; }

        /// <summary>
        /// Invokes a pipeline for given input.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="PipelineInvocationException">Throws when an unhandled exception was thrown during pipeline execution.</exception>
        ValueTask<TArgs> InvokeAsync(TArgs args, CancellationToken cancellationToken = default);
    }
}
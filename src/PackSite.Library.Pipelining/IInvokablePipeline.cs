namespace PackSite.Library.Pipelining
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Invokable pipeline.
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public interface IInvokablePipeline<TParameter>
        where TParameter : class
    {
        /// <summary>
        /// Pipeline.
        /// </summary>
        IPipeline<TParameter> Pipeline { get; }

        /// <summary>
        /// Invokes a pipeline for given input.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="PipelineException">Throws when an unhandled exception was thrown during pipeline execution.</exception>
        ValueTask<TParameter> InvokeAsync(TParameter input, CancellationToken cancellationToken = default);
    }
}
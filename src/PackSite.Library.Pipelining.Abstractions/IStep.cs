namespace PackSite.Library.Pipelining
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Step delegate.
    /// </summary>
    /// <returns></returns>
    public delegate ValueTask StepDelegate();

    /// <summary>
    /// Pipeline step.
    /// </summary>
    public interface IStep : IBaseStep
    {
        /// <summary>
        /// Step entry point.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ExecuteAsync(object context, StepDelegate next, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Pipeline step.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IStep<TContext> : IBaseStep
        where TContext : class
    {
        /// <summary>
        /// Step entry point.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ExecuteAsync(TContext context, StepDelegate next, CancellationToken cancellationToken = default);
    }
}

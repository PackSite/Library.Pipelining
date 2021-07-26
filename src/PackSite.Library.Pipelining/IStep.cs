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
        /// <param name="next"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ExecuteAsync(StepDelegate next, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Pipeline step.
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public interface IStep<TParameter> : IBaseStep
        where TParameter : class
    {
        /// <summary>
        /// Step entry point.
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="next"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ExecuteAsync(TParameter parameter, StepDelegate next, CancellationToken cancellationToken = default);
    }
}

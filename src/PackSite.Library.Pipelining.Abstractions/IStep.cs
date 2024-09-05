namespace PackSite.Library.Pipelining
{
    /// <summary>
    /// Pipeline step.
    /// </summary>
    public interface IStep : IBaseStep
    {
        /// <summary>
        /// Step entry point.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="next"></param>
        /// <param name="invokablePipeline"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExecuteAsync(object args, StepDelegate next, IInvokablePipeline invokablePipeline, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Pipeline step.
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    public interface IStep<TArgs> : IBaseStep
        where TArgs : class
    {
        /// <summary>
        /// Step entry point.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="next"></param>
        /// <param name="invokablePipeline"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ExecuteAsync(TArgs args, StepDelegate next, IInvokablePipeline<TArgs> invokablePipeline, CancellationToken cancellationToken = default);
    }
}

﻿namespace PackSite.Library.Pipelining
{
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
        /// <param name="args"></param>
        /// <param name="next"></param>
        /// <param name="invokablePipeline"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ExecuteAsync(object args, StepDelegate next, IInvokablePipeline invokablePipeline, CancellationToken cancellationToken = default);
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
        ValueTask ExecuteAsync(TArgs args, StepDelegate next, IInvokablePipeline<TArgs> invokablePipeline, CancellationToken cancellationToken = default);
    }
}

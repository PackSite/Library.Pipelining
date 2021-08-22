namespace PackSite.Library.Pipelining
{
    using System;

    /// <summary>
    /// Invokable pipeline collection.
    /// </summary>
    public interface IInvokablePipelineFactory
    {
        /// <summary>
        /// Gets invokable pipeline by name. Returns null when not found.
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        IInvokablePipeline<TArgs>? GetPipeline<TArgs>(PipelineName name)
            where TArgs : class;

        /// <summary>
        /// Gets invokable pipeline by its default name name. Returns null when not found.
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <returns></returns>
        IInvokablePipeline<TArgs>? GetPipeline<TArgs>()
            where TArgs : class;

        /// <summary>
        /// Gets invokable pipeline by name or default.
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="name"/> is invalid or <typeparamref name="TArgs"/> is invalid for a named pipeline.</exception>
        IInvokablePipeline<TArgs> GetRequiredPipeline<TArgs>(PipelineName name)
            where TArgs : class;

        /// <summary>
        /// Gets invokable pipeline by its default name name.
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when pipeline was not found.</exception>
        IInvokablePipeline<TArgs> GetRequiredPipeline<TArgs>()
            where TArgs : class;
    }
}
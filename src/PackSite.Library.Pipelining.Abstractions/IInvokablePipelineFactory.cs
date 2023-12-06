namespace PackSite.Library.Pipelining
{
    /// <summary>
    /// Invokable pipeline factory.
    /// </summary>
    public interface IInvokablePipelineFactory
    {
        /// <summary>
        /// Gets invokable pipeline by name. Returns null when not found.
        ///
        /// When requesting a pipeline for sub-pipelining it's not recommended to retrieve pipeline in constructor.
        /// Instead, retrieve it inside ExecuteAsync.
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        IInvokablePipeline<TArgs>? GetPipeline<TArgs>(PipelineName name)
            where TArgs : class;

        /// <summary>
        /// Gets invokable pipeline by its default name. Returns null when not found.
        ///
        /// When requesting a pipeline for sub-pipelining it's not recommended to retrieve pipeline in constructor.
        /// Instead, retrieve it inside ExecuteAsync.
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <returns></returns>
        IInvokablePipeline<TArgs>? GetPipeline<TArgs>()
            where TArgs : class;

        /// <summary>
        /// Gets invokable pipeline by name or default.
        ///
        /// When requesting a pipeline for sub-pipelining it's not recommended to retrieve pipeline in constructor.
        /// Instead, retrieve it inside ExecuteAsync.
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="name"/> is invalid or <typeparamref name="TArgs"/> is invalid for a named pipeline.</exception>
        IInvokablePipeline<TArgs> GetRequiredPipeline<TArgs>(PipelineName name)
            where TArgs : class;

        /// <summary>
        /// Gets invokable pipeline by its default name.
        ///
        /// When requesting a pipeline for sub-pipelining it's not recommended to retrieve pipeline in constructor.
        /// Instead, retrieve it inside ExecuteAsync.
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when pipeline was not found.</exception>
        IInvokablePipeline<TArgs> GetRequiredPipeline<TArgs>()
            where TArgs : class;
    }
}
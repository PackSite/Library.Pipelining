namespace PackSite.Library.Pipelining
{
    using System;

    /// <summary>
    /// Invokable pipeline collection.
    /// </summary>
    public interface IInvokablePipelineFactory
    {
        /// <summary>
        /// Gets invokable pipeline by name or default.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        IInvokablePipeline<TContext>? GetPipeline<TContext>(PipelineName name)
            where TContext : class;

        /// <summary>
        /// Gets invokable pipeline by name or default.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="name"/> is invalid or <typeparamref name="TContext"/> is invalid for a named pipeline.</exception>
        IInvokablePipeline<TContext> GetRequiredPipeline<TContext>(PipelineName name)
            where TContext : class;
    }
}
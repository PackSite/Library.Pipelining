namespace PackSite.Library.Pipelining
{
    using System;

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IPipelineBuilder<TContext>
        where TContext : class
    {
        /// <summary>
        /// Overrides <see cref="IInvokablePipeline{TContext}"/> lifetime.
        /// </summary>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        IPipelineBuilder<TContext> Lifetime(InvokablePipelineLifetime lifetime);

        /// <summary>
        /// Sets a custom pipeline name.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws when pipeline was built.</exception>
        IPipelineBuilder<TContext> Name(PipelineName name);

        /// <summary>
        /// Sets pipeline description.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="description"/> is null or whitespace.</exception>
        /// <exception cref="InvalidOperationException">Throws when pipeline was built.</exception>
        IPipelineBuilder<TContext> Description(string description);

        /// <summary>
        /// Adds a step to the pipeline.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <typeparamref name="TStep"/> is not <see cref="IStep"/> or <see cref="IStep{TContext}"/>.</exception>
        /// <exception cref="InvalidOperationException">Throws when pipeline was built.</exception>
        IPipelineBuilder<TContext> Add<TStep>()
            where TStep : class, IBaseStep;

        /// <summary>
        /// Adds a step instance to the pipeline.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> step is null.</exception>
        /// <exception cref="ArgumentException">Throws when <typeparamref name="TStep"/> is not <see cref="IStep"/> or <see cref="IStep{TContext}"/>.</exception>
        /// <exception cref="InvalidOperationException">Throws when pipeline was built.</exception>
        IPipelineBuilder<TContext> Add<TStep>(TStep instance)
            where TStep : class, IBaseStep;

        /// <summary>
        /// Creates a pipeline.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws when pipeline was already built.</exception>
        IPipeline<TContext> Build();
    }
}
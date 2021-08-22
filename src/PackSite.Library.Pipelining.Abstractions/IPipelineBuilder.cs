namespace PackSite.Library.Pipelining
{
    using System;

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    public interface IPipelineBuilder
    {
        /// <summary>
        /// Overrides <see cref="IInvokablePipeline{TContext}"/> lifetime (default: <see cref="InvokablePipelineLifetime.Singleton"/>).
        /// </summary>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        IPipelineBuilder Lifetime(InvokablePipelineLifetime lifetime);

        /// <summary>
        /// Sets a custom pipeline name (default: <code>typeof(IPipeline&lt;TContext&gt;).FullName</code>).
        /// </summary>
        /// <returns></returns>
        IPipelineBuilder Name(PipelineName? name);

        /// <summary>
        /// Sets pipeline description.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="description"/> is null.</exception>
        IPipelineBuilder Description(string description);

        /// <summary>
        /// Adds a step to the pipeline.
        /// </summary>
        /// <param name="stepType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="stepType"/> is not <see cref="IStep"/> or <see cref="IStep{TContext}"/>.</exception>
        IPipelineBuilder Add(Type stepType);

        /// <summary>
        /// Adds a step to the pipeline.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <typeparamref name="TStep"/> is not <see cref="IStep"/> or <see cref="IStep{TContext}"/>.</exception>
        IPipelineBuilder Add<TStep>()
            where TStep : class, IBaseStep;

        /// <summary>
        /// Adds a step instance to the pipeline.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when <typeparamref name="TStep"/> is not <see cref="IStep"/> or <see cref="IStep{TContext}"/>.</exception>
        IPipelineBuilder Add<TStep>(TStep instance)
            where TStep : class, IBaseStep;

        /// <summary>
        /// Creates a pipeline.
        /// </summary>
        /// <returns></returns>
        IPipeline Build();
    }

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public interface IPipelineBuilder<TContext> : IPipelineBuilder
        where TContext : class
    {
        /// <summary>
        /// Overrides <see cref="IInvokablePipeline{TContext}"/> lifetime (default: <see cref="InvokablePipelineLifetime.Singleton"/>).
        /// </summary>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        new IPipelineBuilder<TContext> Lifetime(InvokablePipelineLifetime lifetime);

        /// <summary>
        /// Sets a custom pipeline name (default: <code>typeof(IPipeline&lt;TContext&gt;).FullName</code>).
        /// </summary>
        /// <returns></returns>
        new IPipelineBuilder<TContext> Name(PipelineName? name);

        /// <summary>
        /// Sets pipeline description.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="description"/> is null.</exception>
        new IPipelineBuilder<TContext> Description(string description);

        /// <summary>
        /// Adds a step to the pipeline.
        /// </summary>
        /// <param name="stepType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="stepType"/> is not <see cref="IStep"/> or <see cref="IStep{TContext}"/>.</exception>
        new IPipelineBuilder<TContext> Add(Type stepType);

        /// <summary>
        /// Adds a step to the pipeline.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <typeparamref name="TStep"/> is not <see cref="IStep"/> or <see cref="IStep{TContext}"/>.</exception>
        new IPipelineBuilder<TContext> Add<TStep>()
            where TStep : class, IBaseStep;

        /// <summary>
        /// Adds a step instance to the pipeline.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when <typeparamref name="TStep"/> is not <see cref="IStep"/> or <see cref="IStep{TContext}"/>.</exception>
        new IPipelineBuilder<TContext> Add<TStep>(TStep instance)
            where TStep : class, IBaseStep;

        /// <summary>
        /// Creates a pipeline.
        /// </summary>
        /// <returns></returns>
        new IPipeline<TContext> Build();
    }
}
namespace PackSite.Library.Pipelining
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    public interface IPipelineBuilder
    {
        /// <summary>
        /// Overrides <see cref="IInvokablePipeline"/> lifetime (default: <see cref="InvokablePipelineLifetime.Singleton"/>).
        /// </summary>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        IPipelineBuilder Lifetime(InvokablePipelineLifetime lifetime);

        /// <summary>
        /// Sets a custom pipeline name (default: <code>typeof(IPipeline&lt;TArgs&gt;).FullName</code>).
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
        /// <exception cref="ArgumentNullException">Throws when <paramref name="stepType"/> is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/>.</exception>
        IPipelineBuilder Step(Type stepType);

        /// <summary>
        /// Adds a step to the pipeline.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <typeparamref name="TStep"/> is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/>.</exception>
        IPipelineBuilder Step<TStep>()
            where TStep : class, IBaseStep;

        /// <summary>
        /// Adds a step instance to the pipeline.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when <typeparamref name="TStep"/> is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/>.</exception>
        IPipelineBuilder Step<TStep>(TStep instance)
            where TStep : class, IBaseStep;

        /// <summary>
        /// Adds a step instances to the pipeline.
        /// </summary>
        /// <param name="instances"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instances"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder Steps(IEnumerable<IBaseStep> instances);    
        
        /// <summary>
        /// Adds a step instances to the pipeline.
        /// </summary>
        /// <param name="instances"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instances"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder Steps(params IBaseStep[] instances);

        /// <summary>
        /// Creates a pipeline.
        /// </summary>
        /// <returns></returns>
        IPipeline Build();
    }

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    public interface IPipelineBuilder<TArgs> : IPipelineBuilder
        where TArgs : class
    {
        /// <summary>
        /// Overrides <see cref="IInvokablePipeline{TArgs}"/> lifetime (default: <see cref="InvokablePipelineLifetime.Singleton"/>).
        /// </summary>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        new IPipelineBuilder<TArgs> Lifetime(InvokablePipelineLifetime lifetime);

        /// <summary>
        /// Sets a custom pipeline name (default: <code>typeof(IPipeline&lt;TArgs&gt;).FullName</code>).
        /// </summary>
        /// <returns></returns>
        new IPipelineBuilder<TArgs> Name(PipelineName? name);

        /// <summary>
        /// Sets pipeline description.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="description"/> is null.</exception>
        new IPipelineBuilder<TArgs> Description(string description);

        /// <summary>
        /// Adds a step to the pipeline.
        /// </summary>
        /// <param name="stepType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="stepType"/> is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/>.</exception>
        new IPipelineBuilder<TArgs> Step(Type stepType);

        /// <summary>
        /// Adds a step to the pipeline.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <typeparamref name="TStep"/> is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/>.</exception>
        new IPipelineBuilder<TArgs> Step<TStep>()
            where TStep : class, IBaseStep;

        /// <summary>
        /// Adds a step instance to the pipeline.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when <typeparamref name="TStep"/> is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/>.</exception>
        new IPipelineBuilder<TArgs> Step<TStep>(TStep instance)
            where TStep : class, IBaseStep;

        /// <summary>
        /// Adds a step instances to the pipeline.
        /// </summary>
        /// <param name="instances"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instances"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> Steps(IEnumerable<IBaseStep> instances);

        /// <summary>
        /// Adds a step instances to the pipeline.
        /// </summary>
        /// <param name="instances"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instances"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> Steps(params IBaseStep[] instances);

        /// <summary>
        /// Creates a pipeline.
        /// </summary>
        /// <returns></returns>
        new IPipeline<TArgs> Build();
    }
}
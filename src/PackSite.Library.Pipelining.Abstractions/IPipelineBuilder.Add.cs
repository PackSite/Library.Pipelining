﻿namespace PackSite.Library.Pipelining
{
    public partial interface IPipelineBuilder
    {
        /// <summary>
        /// Adds a step to the pipeline.
        /// </summary>
        /// <param name="stepType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="stepType"/> is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/>.</exception>
        IPipelineBuilder Add(Type stepType);

        /// <summary>
        /// Adds a step to the pipeline.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when <typeparamref name="TStep"/> is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/>.</exception>
        IPipelineBuilder Add<TStep>()
            where TStep : class, IBaseStep;
    }

    public partial interface IPipelineBuilder<TArgs>
    {
        /// <summary>
        /// Adds a step instance to the pipeline.
        /// </summary>
        /// <param name="stepType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="stepType"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> Add(Type stepType);

        /// <summary>
        /// Adds a step to the pipeline.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when <typeparamref name="TStep"/> is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/>.</exception>
        new IPipelineBuilder<TArgs> Add<TStep>()
            where TStep : class, IBaseStep;
    }
}
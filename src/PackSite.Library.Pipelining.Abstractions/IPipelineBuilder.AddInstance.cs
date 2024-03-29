﻿namespace PackSite.Library.Pipelining
{
    public partial interface IPipelineBuilder
    {
        /// <summary>
        /// Adds a step instance to the pipeline.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder Add(IBaseStep instance);

        /// <summary>
        /// Adds a step instances to the pipeline.
        /// </summary>
        /// <param name="instances"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instances"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder AddRange(IEnumerable<IBaseStep> instances);

        /// <summary>
        /// Adds a step instances to the pipeline.
        /// </summary>
        /// <param name="instances"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instances"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder AddRange(params IBaseStep[] instances);
    }

    public partial interface IPipelineBuilder<TArgs>
    {
        /// <summary>
        /// Adds a step instance to the pipeline.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> Add(IBaseStep instance);

        /// <summary>
        /// Adds a step instances to the pipeline.
        /// </summary>
        /// <param name="instances"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instances"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> AddRange(IEnumerable<IBaseStep> instances);

        /// <summary>
        /// Adds a step instances to the pipeline.
        /// </summary>
        /// <param name="instances"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instances"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> AddRange(params IBaseStep[] instances);
    }
}
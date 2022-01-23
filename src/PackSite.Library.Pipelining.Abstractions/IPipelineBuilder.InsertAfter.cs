namespace PackSite.Library.Pipelining
{
    using System;

    public partial interface IPipelineBuilder
    {
        /// <summary>
        /// Inserts a step instance to the pipeline after all <typeparamref name="TAfter"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="TAfter"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder InsertAfter<TAfter>(IBaseStep instance)
            where TAfter : class, IBaseStep;

        /// <summary>
        /// Inserts a step instance to the pipeline after all <paramref name="after"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder InsertAfter(IBaseStep instance, Type after);

        /// <summary>
        /// Inserts a step to the pipeline after all <typeparamref name="TAfter"/>.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <typeparam name="TAfter"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder InsertAfter<TStep, TAfter>()
            where TStep : class, IBaseStep
            where TAfter : class, IBaseStep;

        /// <summary>
        /// Inserts a step to the pipeline after all <paramref name="after"/>.>.
        /// </summary>
        /// <param name="step"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder InsertAfter(Type step, Type after);
    }

    public partial interface IPipelineBuilder<TArgs>
    {
        /// <summary>
        /// Inserts a step instance to the pipeline after all <typeparamref name="TAfter"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="TAfter"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> InsertAfter<TAfter>(IBaseStep instance)
            where TAfter : class, IBaseStep;

        /// <summary>
        /// Inserts a step instance to the pipeline after all <paramref name="after"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> InsertAfter(IBaseStep instance, Type after);

        /// <summary>
        /// Inserts a step to the pipeline after all <typeparamref name="TAfter"/>.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <typeparam name="TAfter"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> InsertAfter<TStep, TAfter>()
            where TStep : class, IBaseStep
            where TAfter : class, IBaseStep;

        /// <summary>
        /// Inserts a step to the pipeline after all <paramref name="after"/>..
        /// </summary>
        /// <param name="step"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> InsertAfter(Type step, Type after);
    }
}
namespace PackSite.Library.Pipelining
{
    public partial interface IPipelineBuilder
    {
        /// <summary>
        /// Inserts a step instance to the pipeline before all <typeparamref name="TBefore"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="TBefore"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder InsertBefore<TBefore>(IBaseStep instance)
            where TBefore : class, IBaseStep;

        /// <summary>
        /// Inserts a step instance to the pipeline before all <paramref name="before"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="before"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder InsertBefore(IBaseStep instance, Type before);

        /// <summary>
        /// Inserts a step to the pipeline before all <typeparamref name="TBefore"/>.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <typeparam name="TBefore"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder InsertBefore<TStep, TBefore>()
            where TStep : class, IBaseStep
            where TBefore : class, IBaseStep;

        /// <summary>
        /// Inserts a step to the pipeline before all <paramref name="before"/>.>.
        /// </summary>
        /// <param name="step"></param>
        /// <param name="before"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder InsertBefore(Type step, Type before);
    }

    public partial interface IPipelineBuilder<TArgs>
    {
        /// <summary>
        /// Inserts a step instance to the pipeline before all <typeparamref name="TBefore"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="TBefore"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> InsertBefore<TBefore>(IBaseStep instance)
            where TBefore : class, IBaseStep;

        /// <summary>
        /// Inserts a step instance to the pipeline before all <paramref name="before"/>.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="before"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> InsertBefore(IBaseStep instance, Type before);

        /// <summary>
        /// Inserts a step to the pipeline before all <typeparamref name="TBefore"/>.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <typeparam name="TBefore"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> InsertBefore<TStep, TBefore>()
            where TStep : class, IBaseStep
            where TBefore : class, IBaseStep;

        /// <summary>
        /// Inserts a step to the pipeline before all <paramref name="before"/>..
        /// </summary>
        /// <param name="step"></param>
        /// <param name="before"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> InsertBefore(Type step, Type before);
    }
}
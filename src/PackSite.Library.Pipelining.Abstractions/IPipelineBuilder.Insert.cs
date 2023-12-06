namespace PackSite.Library.Pipelining
{
    public partial interface IPipelineBuilder
    {
        /// <summary>
        /// Inserts a step instance to the pipeline.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder Insert(int index, IBaseStep instance);

        /// <summary>
        /// Inserts a step to the pipeline.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder Insert<TStep>(int index)
            where TStep : class, IBaseStep;

        /// <summary>
        /// Inserts a step to the pipeline.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        IPipelineBuilder Insert(int index, Type step);
    }

    public partial interface IPipelineBuilder<TArgs>
    {
        /// <summary>
        /// Inserts a step instance to the pipeline.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="instance"/> is null.</exception>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> Insert(int index, IBaseStep instance);

        /// <summary>
        /// Inserts a step to the pipeline.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> Insert<TStep>(int index)
            where TStep : class, IBaseStep;

        /// <summary>
        /// Inserts a step to the pipeline.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when an instance that is not <see cref="IStep"/> or <see cref="IStep{TArgs}"/> was found.</exception>
        new IPipelineBuilder<TArgs> Insert(int index, Type step);
    }
}
namespace PackSite.Library.Pipelining
{
    using System;

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public interface IPipelineBuilder<TParameter>
        where TParameter : class
    {
        /// <summary>
        /// Sets a custom pipeline name.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="name"/> is null or whitespace.</exception>
        /// <exception cref="InvalidOperationException">Throws when pipeline was built.</exception>
        IPipelineBuilder<TParameter> Name(string name);

        /// <summary>
        /// Sets pipeline description.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throws when <paramref name="description"/> is null or whitespace.</exception>
        /// <exception cref="InvalidOperationException">Throws when pipeline was built.</exception>
        IPipelineBuilder<TParameter> Description(string description);

        /// <summary>
        /// Adds a step to the pipeline.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws when pipeline was built.</exception>
        IPipelineBuilder<TParameter> Add<TStep>()
            where TStep : class, IBaseStep;

        /// <summary>
        /// Adds a step instance to the pipeline.
        /// </summary>
        /// <typeparam name="TStep"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws when pipeline was built.</exception>
        IPipelineBuilder<TParameter> Add<TStep>(TStep instance)
            where TStep : class, IBaseStep;

        /// <summary>
        /// Creates a pipeline.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws when pipeline was already built.</exception>
        IPipeline<TParameter> Build();
    }
}
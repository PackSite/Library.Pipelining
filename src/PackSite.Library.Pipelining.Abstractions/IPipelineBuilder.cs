namespace PackSite.Library.Pipelining
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Pipeline builder.
    /// </summary>
    public partial interface IPipelineBuilder : IList<Type>
    {
        /// <summary>
        /// Overrides <see cref="IInvokablePipeline"/> lifetime (default: <see cref="InvokablePipelineLifetime.Singleton"/>).
        /// </summary>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        IPipelineBuilder Lifetime(InvokablePipelineLifetime lifetime);

        /// <summary>
        /// Sets a custom pipeline name (default: <code>typeof(IPipeline&lt;TArgs&gt;).FullName</code>).
        /// Pass null to reset name to default.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IPipelineBuilder Name(PipelineName? name);

        /// <summary>
        /// Sets pipeline description.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="description"/> is null.</exception>
        IPipelineBuilder Description(string description);

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
    public partial interface IPipelineBuilder<TArgs> : IPipelineBuilder
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
        /// Creates a pipeline.
        /// </summary>
        /// <returns></returns>
        new IPipeline<TArgs> Build();
    }
}
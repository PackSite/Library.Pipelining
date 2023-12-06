namespace PackSite.Library.Pipelining
{
    /// <summary>
    /// Pipeline.
    /// </summary>
    public interface IPipeline : IFormattable
    {
        /// <summary>
        /// Pipeline instance counters.
        /// </summary>
        IPipelineCounters Counters { get; }

        /// <summary>
        /// Pipeline lifetime.
        /// </summary>
        InvokablePipelineLifetime Lifetime { get; }

        /// <summary>
        /// Pipeline name.
        /// </summary>
        PipelineName Name { get; }

        /// <summary>
        /// Pipeline description.
        /// </summary>
        string? Description { get; }

        /// <summary>
        /// Steps.
        /// </summary>
        IReadOnlyList<Type> Steps { get; }

        /// <summary>
        /// Creates an invokable pipeline.
        /// </summary>
        /// <param name="stepActivator"></param>
        /// <returns></returns>
        IInvokablePipeline CreateInvokable(IBaseStepActivator stepActivator);

        /// <summary>
        /// Creates a builder instance.
        /// </summary>
        /// <returns></returns>
        IPipelineBuilder CreateBuilder();
    }

    /// <summary>
    /// Pipeline.
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    public interface IPipeline<TArgs> : IPipeline
        where TArgs : class
    {
        /// <summary>
        /// Default pipeline name.
        /// </summary>
        public static PipelineName DefaultName => $"Pipeline<{typeof(TArgs)}>";

        /// <summary>
        /// Creates an invokable pipeline.
        /// </summary>
        /// <param name="stepActivator"></param>
        /// <returns></returns>
        new IInvokablePipeline<TArgs> CreateInvokable(IBaseStepActivator stepActivator);

        /// <summary>
        /// Creates a builder instance.
        /// </summary>
        /// <returns></returns>
        new IPipelineBuilder<TArgs> CreateBuilder();
    }
}
namespace PackSite.Library.Pipelining
{
    /// <summary>
    /// Pipeline.
    /// </summary>
    public interface IPipeline
    {
        /// <summary>
        /// Pipeline name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Pipeline description.
        /// </summary>
        string? Description { get; }
    }

    /// <summary>
    /// Pipeline.
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public interface IPipeline<TParameter> : IPipeline
        where TParameter : class
    {
        /// <summary>
        /// Creates an invokable pipeline.
        /// </summary>
        /// <param name="stepActivator"></param>
        /// <returns></returns>
        IInvokablePipeline<TParameter> AsInvokable(IStepActivator stepActivator);
    }
}
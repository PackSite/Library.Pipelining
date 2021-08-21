namespace PackSite.Library.Pipelining
{
    using PackSite.Library.Pipelining.Internal;

    /// <summary>
    /// Pipeline builder helper.
    /// </summary>
    public static class PipelineBuilder
    {
        /// <summary>
        /// Creates a new instance of pipeline builder.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static IPipelineBuilder<TContext> Create<TContext>()
            where TContext : class
        {
            return new PipelineBuilder<TContext>();
        }
    }
}

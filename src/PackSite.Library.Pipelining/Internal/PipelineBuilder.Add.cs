namespace PackSite.Library.Pipelining.Internal
{
    using PackSite.Library.Pipelining;

    internal sealed partial class PipelineBuilder<TArgs>
    {
        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> Add(Type stepType)
        {
            Steps.Add(stepType);

            return this;
        }

        IPipelineBuilder IPipelineBuilder.Add(Type stepType)
        {
            Steps.Add(stepType);

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> Add<TStep>()
            where TStep : class, IBaseStep
        {
            Steps.Add(typeof(TStep));

            return this;
        }

        IPipelineBuilder IPipelineBuilder.Add<TStep>()
        {
            Steps.Add(typeof(TStep));

            return this;
        }
    }
}

namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using PackSite.Library.Pipelining;

    internal sealed partial class PipelineBuilder<TArgs>
    {
        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> AddStep(Type stepType)
        {
            Add(stepType);

            return this;
        }

        IPipelineBuilder IPipelineBuilder.AddStep(Type stepType)
        {
            Add(stepType);

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> AddStep<TStep>()
            where TStep : class, IBaseStep
        {
            Add(typeof(TStep));

            return this;
        }

        IPipelineBuilder IPipelineBuilder.AddStep<TStep>()
        {
            Add(typeof(TStep));

            return this;
        }
    }
}

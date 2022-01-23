namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using PackSite.Library.Pipelining;

    internal sealed partial class PipelineBuilder<TArgs>
    {
        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> Insert(int index, IBaseStep instance)
        {
            _ = instance ?? throw new ArgumentNullException(nameof(instance));

            if (instance is not (IStep or IStep<TArgs>))
            {
                throw new ArgumentException($"Invalid step instance type '{instance.GetType().FullName}'.", nameof(instance));
            }

            Steps.Insert(index, instance);

            return this;
        }

        IPipelineBuilder IPipelineBuilder.Insert(int index, IBaseStep instance)
        {
            return Insert(index, instance);
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> Insert<TStep>(int index)
            where TStep : class, IBaseStep
        {
            Steps.Insert(index, typeof(TStep));

            return this;
        }

        IPipelineBuilder IPipelineBuilder.Insert<TStep>(int index)
        {
            Steps.Insert(index, typeof(TStep));

            return this;
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> Insert(int index, Type step)
        {
            Steps.Insert(index, step);

            return this;
        }

        IPipelineBuilder IPipelineBuilder.Insert(int index, Type step)
        {
            Steps.Insert(index, step);

            return this;
        }
    }
}

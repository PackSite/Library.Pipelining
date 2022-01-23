namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections.Generic;
    using PackSite.Library.Pipelining;

    internal sealed partial class PipelineBuilder<TArgs>
    {
        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> Add(IBaseStep instance)
        {
            _ = instance ?? throw new ArgumentNullException(nameof(instance));

            if (instance is not (IStep or IStep<TArgs>))
            {
                throw new ArgumentException($"Invalid step instance type '{instance.GetType().FullName}'.", nameof(instance));
            }

            Steps.Add(instance);

            return this;
        }

        IPipelineBuilder IPipelineBuilder.Add(IBaseStep instance)
        {
            return Add(instance);
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> AddRange(IEnumerable<IBaseStep> instances)
        {
            foreach (IBaseStep instance in instances)
            {
                Add(instance);
            }

            return this;
        }

        IPipelineBuilder IPipelineBuilder.AddRange(IEnumerable<IBaseStep> instances)
        {
            return AddRange(instances);
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> AddRange(params IBaseStep[] instances)
        {
            for (int i = 0; i < instances.Length; i++)
            {
                Add(instances[i]);
            }

            return this;
        }

        IPipelineBuilder IPipelineBuilder.AddRange(params IBaseStep[] instances)
        {
            return AddRange(instances);
        }
    }
}

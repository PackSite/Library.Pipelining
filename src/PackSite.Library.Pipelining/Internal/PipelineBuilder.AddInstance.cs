namespace PackSite.Library.Pipelining.Internal
{
    using PackSite.Library.Pipelining;

    internal sealed partial class PipelineBuilder<TArgs>
    {
        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> Add(IBaseStep instance)
        {
            ArgumentNullException.ThrowIfNull(instance);

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
            ArgumentNullException.ThrowIfNull(instances);

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
            ArgumentNullException.ThrowIfNull(instances);

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

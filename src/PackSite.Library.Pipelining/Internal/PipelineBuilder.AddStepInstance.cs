namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections.Generic;
    using PackSite.Library.Pipelining;

    internal sealed partial class PipelineBuilder<TArgs>
    {
        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> AddStep(IBaseStep instance)
        {
            _ = instance ?? throw new ArgumentNullException(nameof(instance));

            if (instance is not (IStep or IStep<TArgs>))
            {
                throw new ArgumentException($"Invalid step instance type '{instance.GetType().FullName}'.", nameof(instance));
            }

            _steps.Add(instance);
            _stepTypes.Add(instance.GetType());

            return this;
        }

        IPipelineBuilder IPipelineBuilder.AddStep(IBaseStep instance)
        {
            return AddStep(instance);
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> AddSteps(IEnumerable<IBaseStep> instances)
        {
            foreach (IBaseStep instance in instances)
            {
                AddStep(instance);
            }

            return this;
        }

        IPipelineBuilder IPipelineBuilder.AddSteps(IEnumerable<IBaseStep> instances)
        {
            return AddSteps(instances);
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> AddSteps(params IBaseStep[] instances)
        {
            for (int i = 0; i < instances.Length; i++)
            {
                AddStep(instances[i]);
            }

            return this;
        }

        IPipelineBuilder IPipelineBuilder.AddSteps(params IBaseStep[] instances)
        {
            return AddSteps(instances);
        }
    }
}

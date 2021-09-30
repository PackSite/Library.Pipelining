namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using PackSite.Library.Pipelining;

    internal sealed partial class PipelineBuilder<TArgs>
    {
        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> InsertStep(int index, IBaseStep instance)
        {
            _ = instance ?? throw new ArgumentNullException(nameof(instance));

            if (instance is not (IStep or IStep<TArgs>))
            {
                throw new ArgumentException($"Invalid step instance type '{instance.GetType().FullName}'.", nameof(instance));
            }

            _steps.Insert(index, instance);
            _stepTypes.Insert(index, instance.GetType());

            return this;
        }

        IPipelineBuilder IPipelineBuilder.InsertStep(int index, IBaseStep instance)
        {
            return InsertStep(index, instance);
        }

        /// <inheritdoc/>
        public IPipelineBuilder<TArgs> InsertStep<TStep>(int index)
            where TStep : class, IBaseStep
        {
            Insert(index, typeof(TStep));

            return this;
        }

        IPipelineBuilder IPipelineBuilder.InsertStep<TStep>(int index)
        {
            Insert(index, typeof(TStep));

            return this;
        }
    }
}

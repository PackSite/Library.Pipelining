namespace PackSite.Library.Pipelining.Internal
{
    using PackSite.Library.Pipelining;

    internal sealed partial class PipelineBuilder<TArgs>
    {
        IPipelineBuilder<TArgs> IPipelineBuilder<TArgs>.InsertAfter<TAfter>(IBaseStep instance)
        {
            return InsertAfter(instance, typeof(TAfter));
        }

        public IPipelineBuilder<TArgs> InsertAfter(IBaseStep instance, Type after)
        {
            Type instanceType = instance.GetType();
            if (instanceType == after)
            {
                throw new ArgumentException($"Cannot insert step instance of type '{instanceType}' after '{after}'.", nameof(after));
            }

            int lastIndex = -1, index;
            int originalCount = Steps.Count;

            for (int i = 0; i < originalCount; i++)
            {
                index = Steps.IndexOf(after, lastIndex + 1);

                if (index < 0 || index == lastIndex)
                {
                    break;
                }

                lastIndex = index;

                Insert(index + 1, instance);
            }

            return this;
        }

        IPipelineBuilder<TArgs> IPipelineBuilder<TArgs>.InsertAfter<TStep, TAfter>()
        {
            return InsertAfter(typeof(TStep), typeof(TAfter));
        }

        public IPipelineBuilder<TArgs> InsertAfter(Type step, Type after)
        {
            if (step == after)
            {
                throw new ArgumentException($"Cannot insert step of type '{step}' after '{after}'.", nameof(step));
            }

            int lastIndex = -1, index;
            int originalCount = Steps.Count;

            for (int i = 0; i < originalCount; i++)
            {
                index = Steps.IndexOf(after, lastIndex + 1);

                if (index < 0 || index == lastIndex)
                {
                    break;
                }

                lastIndex = index;

                Insert(index + 1, step);
            }

            return this;
        }

        IPipelineBuilder IPipelineBuilder.InsertAfter<TAfter>(IBaseStep instance)
        {
            return InsertAfter(instance, typeof(TAfter));
        }

        IPipelineBuilder IPipelineBuilder.InsertAfter(IBaseStep instance, Type after)
        {
            return InsertAfter(instance, after);
        }

        IPipelineBuilder IPipelineBuilder.InsertAfter<TStep, TAfter>()
        {
            return InsertAfter(typeof(TStep), typeof(TAfter));
        }

        IPipelineBuilder IPipelineBuilder.InsertAfter(Type step, Type after)
        {
            return InsertAfter(step, after);
        }
    }
}

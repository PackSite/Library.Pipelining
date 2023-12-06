namespace PackSite.Library.Pipelining.Internal
{
    using PackSite.Library.Pipelining;

    internal sealed partial class PipelineBuilder<TArgs>
    {
        IPipelineBuilder<TArgs> IPipelineBuilder<TArgs>.InsertBefore<TBefore>(IBaseStep instance)
        {
            return InsertBefore(instance, typeof(TBefore));
        }

        public IPipelineBuilder<TArgs> InsertBefore(IBaseStep instance, Type before)
        {
            Type instanceType = instance.GetType();
            if (instanceType == before)
            {
                throw new ArgumentException($"Cannot insert step instance of type '{instanceType}' before '{before}'.");
            }

            int lastIndex = -1, index;
            int originalCount = Steps.Count;

            for (int i = 0; i < originalCount; i++)
            {
                index = Steps.IndexOf(before, lastIndex + 1);

                if (index < 0 || index == lastIndex)
                {
                    break;
                }

                lastIndex = index + 1;

                Insert(index, instance);
            }

            return this;
        }

        IPipelineBuilder<TArgs> IPipelineBuilder<TArgs>.InsertBefore<TStep, TBefore>()
        {
            return InsertBefore(typeof(TStep), typeof(TBefore));
        }

        public IPipelineBuilder<TArgs> InsertBefore(Type step, Type before)
        {
            if (step == before)
            {
                throw new ArgumentException($"Cannot insert step of type '{step}' before '{before}'.");
            }

            int lastIndex = -1, index;
            int originalCount = Steps.Count;

            for (int i = 0; i < originalCount; i++)
            {
                index = Steps.IndexOf(before, lastIndex + 1);

                if (index < 0 || index == lastIndex)
                {
                    break;
                }

                lastIndex = index + 1;

                Insert(index, step);
            }

            return this;
        }

        IPipelineBuilder IPipelineBuilder.InsertBefore<TBefore>(IBaseStep instance)
        {
            return InsertBefore(instance, typeof(TBefore));
        }

        IPipelineBuilder IPipelineBuilder.InsertBefore(IBaseStep instance, Type before)
        {
            return InsertBefore(instance, before);
        }

        IPipelineBuilder IPipelineBuilder.InsertBefore<TStep, TBefore>()
        {
            return InsertBefore(typeof(TStep), typeof(TBefore));
        }

        IPipelineBuilder IPipelineBuilder.InsertBefore(Type step, Type before)
        {
            return InsertBefore(step, before);
        }
    }
}

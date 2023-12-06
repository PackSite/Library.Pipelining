namespace PackSite.Library.Pipelining
{
    using Microsoft.Extensions.DependencyInjection;
    using PackSite.Library.Pipelining.Internal;

    /// <summary>
    /// Pipeline builder helper.
    /// </summary>
    public static class PipelineBuilder
    {
        private static readonly ConcurrentDictionary<Type, ObjectFactory> _cache = new();
        private static readonly IServiceProvider _serviceProvider = NoOpServiceProvider.Instance;

        /// <summary>
        /// Creates a new instance of pipeline builder.
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <returns></returns>
        public static IPipelineBuilder<TArgs> Create<TArgs>()
            where TArgs : class
        {
            return new PipelineBuilder<TArgs>();
        }

        /// <summary>
        /// Creates a new instance of pipeline builder.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IPipelineBuilder Create(Type type)
        {
            ObjectFactory factory = _cache.GetOrAdd(type, static (key) => // The factory may run multiple times but we don't care since we don't want to add overhead with lazy
            {
                Type builderType = typeof(PipelineBuilder<>).MakeGenericType(key);
                return ActivatorUtilities.CreateFactory(builderType, []);
            });

            return (IPipelineBuilder)factory(_serviceProvider, null);
        }
    }
}

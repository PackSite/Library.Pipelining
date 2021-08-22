namespace PackSite.Library.Pipelining
{
    using System;
    using System.Collections.Concurrent;
    using Microsoft.Extensions.DependencyInjection;
    using PackSite.Library.Pipelining.Internal;

    /// <summary>
    /// Pipeline builder helper.
    /// </summary>
    public static class PipelineBuilder
    {
        private static readonly ConcurrentDictionary<Type, ObjectFactory> _cache = new();

        /// <summary>
        /// Creates a new instance of pipeline builder.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static IPipelineBuilder<TContext> Create<TContext>()
            where TContext : class
        {
            return new PipelineBuilder<TContext>();
        }

        /// <summary>
        /// Creates a new instance of pipeline builder.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IPipelineBuilder Create(Type type)
        {
            ObjectFactory factory = _cache.GetOrAdd(type, (key) =>
            {
                Type builderType = typeof(PipelineBuilder<>).MakeGenericType(key);
                return ActivatorUtilities.CreateFactory(builderType, Array.Empty<Type>());
            });

            return (IPipelineBuilder)factory(null!, null);
        }
    }
}

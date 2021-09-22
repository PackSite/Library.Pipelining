namespace PackSite.Library.Pipelining
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using PackSite.Library.Pipelining.Internal;

    /// <summary>
    /// Pipelining extensions.
    /// </summary>
    public static class PipeliningBuilderExtensions
    {
        /// <summary>
        /// Adds pipeline initalizer.
        /// </summary>
        /// <param name="pipelining"></param>
        /// <typeparam name="TInitializer"></typeparam>
        /// <returns></returns>
        public static IServiceCollection AddInitializer<TInitializer>(this PipeliningBuilder pipelining)
            where TInitializer : class, IPipelineInitializer
        {
            return pipelining.Services.AddScoped<IPipelineInitializer, TInitializer>();
        }

        /// <summary>
        /// Adds pipeline initalizer.
        /// </summary>
        /// <param name="pipelining"></param>
        /// <param name="initializer"></param>
        /// <returns></returns>
        public static IServiceCollection AddInitializer(this PipeliningBuilder pipelining, Action<IPipelineCollection> initializer)
        {
            return pipelining.Services.AddScoped<IPipelineInitializer>((provider) => new PipelineDelegateInitializerProxy.Simple(initializer));
        }

        /// <summary>
        /// Adds pipeline initalizer.
        /// </summary>
        /// <param name="pipelining"></param>
        /// <param name="initializer"></param>
        /// <returns></returns>
        public static IServiceCollection AddInitializer(this PipeliningBuilder pipelining, Func<IServiceProvider, IPipelineCollection, CancellationToken, ValueTask>? initializer)
        {
            return pipelining.Services.AddScoped<IPipelineInitializer>((provider) => new PipelineDelegateInitializerProxy.Complex(provider, initializer));
        }
    }
}

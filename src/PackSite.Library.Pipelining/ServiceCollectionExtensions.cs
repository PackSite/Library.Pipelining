namespace PackSite.Library.Pipelining
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using PackSite.Library.Pipelining.Internal;
    using PackSite.Library.Pipelining.StepActivators;

    /// <summary>
    /// Service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds pipelining.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IServiceCollection AddPipelining(this IServiceCollection services, Action<PipeliningBuilder>? builder = null)
        {
            services.TryAddScoped<IStepActivator, ServicesStepActivator>();

            services.TryAddSingleton<IPipelineCollection, PipelineCollection>();
            services.TryAddSingleton<InvokablePipelineFactory.SingletonPipelines>();
            services.TryAddScoped<IInvokablePipelineFactory, InvokablePipelineFactory>();

            services.AddHostedService<PipeliningHostedService>();

            builder?.Invoke(new PipeliningBuilder(services));

            return services;
        }
    }
}

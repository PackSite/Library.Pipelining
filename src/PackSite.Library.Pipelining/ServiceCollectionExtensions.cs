namespace PackSite.Library.Pipelining
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
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
            PipeliningBuilder p = new(services);

            if (!services.Any(x => x.ServiceType == typeof(InvokablePipelineFactory.SingletonPipelines))) // Single check instad of three TryAdd... calls
            {
                services.AddSingleton<IPipelineCollection, PipelineCollection>();
                services.AddSingleton<InvokablePipelineFactory.SingletonPipelines>();
                services.AddScoped<IInvokablePipelineFactory, InvokablePipelineFactory>();

                services.AddHostedService<PipeliningHostedService>();

                p.AddStepActivator<ServicesStepActivator>();
            }

            builder?.Invoke(p);

            return services;
        }
    }
}

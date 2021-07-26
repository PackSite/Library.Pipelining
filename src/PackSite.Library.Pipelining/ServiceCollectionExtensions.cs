namespace PackSite.Library.Pipelining
{
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
        /// <returns></returns>
        public static IServiceCollection AddPipelining(this IServiceCollection services)
        {
            return services.AddPipelining<ServicesStepActivator>(); //TODO: Add Action<> to support adding pipelines during service registration.
        }

        /// <summary>
        /// Adds pipelining with custom step activator.
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddPipelining<TStepActivator>(this IServiceCollection services)
            where TStepActivator : class, IStepActivator
        {
            services.AddScoped<IStepActivator, TStepActivator>();

            services.AddSingleton<IPipelineCollection, PipelineCollection>()
                    .AddSingleton<InvokablePipelineFactory.SingletonPipelines>()
                    .AddScoped<IInvokablePipelineFactory, InvokablePipelineFactory>();

            return services;
        }
    }
}

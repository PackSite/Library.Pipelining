namespace PackSite.Library.Pipelining
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using PackSite.Library.Pipelining.Configuration;
    using PackSite.Library.Pipelining.Internal;
    using PackSite.Library.Pipelining.StepActivators;

    /// <summary>
    /// Service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds pipeline initalizer.
        /// </summary>
        /// <param name="services"></param>
        /// <typeparam name="TInitializer"></typeparam>
        /// <returns></returns>
        public static IServiceCollection AddPipelineInitializer<TInitializer>(this IServiceCollection services)
            where TInitializer : class, IPipelineInitializer
        {
            return services.AddSingleton<IPipelineInitializer, TInitializer>();
        }

        /// <summary>
        /// Adds pipelining with default options.
        /// You can set options from configuration with:
        /// <code>services.Configure&lt;PipeliningConfiguration&gt;(Configuration.GetSection("Pipelining"));</code>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPipelining(this IServiceCollection services)
        {
            services.AddOptions<PipeliningConfiguration>();

            AddCoreServices(services);

            return services;
        }

        /// <summary>
        /// Adds pipelining with default options with default options that can be overriden with configuration action.
        /// You can set options from configuration with:
        /// <code>services.Configure&lt;PipeliningConfiguration&gt;(Configuration.GetSection("Pipelining"));</code>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddPipelining(this IServiceCollection services, Action<PipeliningConfiguration> config)
        {
            services.AddOptions<PipeliningConfiguration>()
                    .PostConfigure(config);

            AddCoreServices(services);

            return services;
        }

        private static void AddCoreServices(IServiceCollection services)
        {
            //services.AddSingleton<IValidateOptions<PipeliningConfiguration>, PipeliningConfigurationValidator>();

            services.TryAddScoped<IStepActivator, ServicesStepActivator>();

            services.AddSingleton<IPipelineCollection, PipelineCollection>()
                    .AddSingleton<InvokablePipelineFactory.SingletonPipelines>()
                    .AddScoped<IInvokablePipelineFactory, InvokablePipelineFactory>();

            services.AddHostedService<PipeliningHostedService>();
        }
    }
}

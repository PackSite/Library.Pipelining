namespace PackSite.Library.Pipelining.Validation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using PackSite.Library.Pipelining.Validation.Internal.Services;
    using PackSite.Library.Pipelining.Validation.Validators;

    /// <summary>
    /// Pipelining extensions.
    /// </summary>
    public static class PipeliningBuilderExtensions
    {
        /// <summary>
        /// Adds a pipeline validator as a singleton service.
        /// Also adds core services on a first call of this method.
        /// </summary>
        /// <param name="pipelining"></param>
        /// <typeparam name="TValidator"></typeparam>
        /// <returns></returns>
        public static PipeliningBuilder AddValidator<TValidator>(this PipeliningBuilder pipelining)
            where TValidator : class, IValidator
        {
            IServiceCollection services = pipelining.Services;

            pipelining.TryAddCoreServices();

            services.AddSingleton<TValidator>();
            services.AddSingleton<IValidator>(provider => provider.GetRequiredService<TValidator>());

            return pipelining;
        }

        private static void TryAddCoreServices(this PipeliningBuilder pipelining)
        {
            IServiceCollection services = pipelining.Services;

            if (!pipelining.IsSubsequentCall)
            {
                services.AddOptions<PipelinesValidationOptions>();
                services.AddHostedService<PipelinesValidationHostedService>();
                services.AddSingleton<IPipelinesValidationService, PipelinesValidationService>();
            }
        }

        /// <summary>
        /// Adds a pipeline validator as a singleton service.
        /// Also adds core services on a first call of this method.
        /// </summary>
        /// <param name="pipelining"></param>
        /// <param name="validator"></param>
        /// <typeparam name="TValidator"></typeparam>
        /// <returns></returns>
        public static PipeliningBuilder AddValidator<TValidator>(this PipeliningBuilder pipelining, TValidator validator)
            where TValidator : class, IValidator
        {
            IServiceCollection services = pipelining.Services;

            pipelining.TryAddCoreServices();

            services.AddSingleton<TValidator>(validator);
            services.AddSingleton<IValidator>(provider => provider.GetRequiredService<TValidator>());

            return pipelining;
        }

        /// <summary>
        /// Adds an inline pipeline validator as a singleton service.
        /// Also adds core services on a first call of this method.
        /// </summary>
        /// <param name="pipelining"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static PipeliningBuilder AddValidator(this PipeliningBuilder pipelining, Action<ValidatorContext> action)
        {
            return pipelining.AddValidator(new InlineValidator(action));
        }

        /// <summary>
        /// Adds a pipeline validator as a singleton service.
        /// Also adds core services on a first call of this method.
        /// </summary>
        /// <param name="pipelining"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static PipeliningBuilder AddValidator(this PipeliningBuilder pipelining, Func<ValidatorContext, CancellationToken, ValueTask> func)
        {
            return pipelining.AddValidator(new InlineValidator(func));
        }
    }
}

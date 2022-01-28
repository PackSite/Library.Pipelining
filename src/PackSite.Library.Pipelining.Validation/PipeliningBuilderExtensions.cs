namespace PackSite.Library.Pipelining.Validation
{
    using Microsoft.Extensions.DependencyInjection;
    using PackSite.Library.Pipelining.Validation.Internal.Services;

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
            where TValidator : class, IPipelineValidator
        {
            IServiceCollection services = pipelining.Services;

            if (!pipelining.IsSubsequentCall)
            {
                services.AddOptions<PipelinesValidationOptions>();
                services.AddHostedService<PipelinesValidationHostedService>();
                services.AddSingleton<IPipelinesValidationService, PipelinesValidationService>();
            }

            services.AddSingleton<TValidator>();
            services.AddSingleton<IPipelineValidator>(provider => provider.GetRequiredService<TValidator>());

            return pipelining;
        }
    }
}

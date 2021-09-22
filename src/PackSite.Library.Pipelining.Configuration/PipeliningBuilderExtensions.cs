namespace PackSite.Library.Pipelining.Configuration
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining.Configuration.Internal;

    /// <summary>
    /// Pipelining extensions.
    /// </summary>
    public static class PipeliningBuilderExtensions
    {
        /// <summary>
        /// Adds pipelining configuration with default options.
        /// Pipelines from configuration are always added after pipelines added/updated by initializer.
        /// You can set options from configuration with:
        /// <code>services.Configure&lt;PipeliningConfiguration&gt;(Configuration.GetSection("Pipelining"));</code>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static PipeliningBuilder AddConfiguration(this PipeliningBuilder builder)
        {
            return builder.AddConfiguration(configruration => { });
        }

        /// <summary>
        /// Adds pipelining configuration with default options with default options that can be overriden with configuration action.
        /// Pipelines from configuration are always added after pipelines added/updated by initializer.
        /// You can set options from configuration with:
        /// <code>services.Configure&lt;PipeliningConfiguration&gt;(Configuration.GetSection("Pipelining"));</code>
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static PipeliningBuilder AddConfiguration(this PipeliningBuilder builder, Action<PipeliningConfiguration> config)
        {
            builder.Services.AddOptions<PipeliningConfiguration>()
                            .PostConfigure(config);

            builder.Services.AddSingleton<IValidateOptions<PipeliningConfiguration>, PipeliningConfigurationValidator>()
                            .AddHostedService<PipeliningConfigurationHostedService>();

            return builder;
        }
    }
}

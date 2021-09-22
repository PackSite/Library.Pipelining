namespace PackSite.Library.Pipelining
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Pipelining extension registration root aka. pipeling builder.
    /// </summary>
    public sealed class PipeliningBuilder
    {
        /// <summary>
        /// Service collection.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="PipeliningBuilder"/>.
        /// </summary>
        /// <param name="services"></param>
        internal PipeliningBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}

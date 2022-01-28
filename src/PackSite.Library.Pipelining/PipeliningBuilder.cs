namespace PackSite.Library.Pipelining
{
    using System;
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
        /// Whether this instance was created as a result of a subsequernt call to <see cref="ServiceCollectionExtensions.AddPipelining(IServiceCollection, System.Action{PipeliningBuilder}?)"/>.
        /// </summary>
        public bool IsSubsequentCall { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="PipeliningBuilder"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="isSubsequentCall"></param>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="services"/> is null.</exception>
        public PipeliningBuilder(IServiceCollection services, bool isSubsequentCall)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            IsSubsequentCall = isSubsequentCall;
        }
    }
}

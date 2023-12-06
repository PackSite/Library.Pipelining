namespace PackSite.Library.Pipelining
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Pipelining extension registration root aka. pipelining builder.
    /// </summary>
    public sealed class PipeliningBuilder
    {
        /// <summary>
        /// Service collection.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Whether this instance was created as a result of a subsequent call to <see cref="ServiceCollectionExtensions.AddPipelining(IServiceCollection, Action{PipeliningBuilder}?)"/>.
        /// </summary>
        public bool IsSubsequentCall { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="PipeliningBuilder"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="isSubsequentCall"></param>
        internal PipeliningBuilder(IServiceCollection services, bool isSubsequentCall)
        {
            Services = services;
            IsSubsequentCall = isSubsequentCall;
        }
    }
}

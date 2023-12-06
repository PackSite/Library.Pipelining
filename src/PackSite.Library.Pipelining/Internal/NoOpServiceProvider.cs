namespace PackSite.Library.Pipelining.Internal
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// No-op service provider.
    /// </summary>
    internal sealed class NoOpServiceProvider : IKeyedServiceProvider
    {
        private static readonly int _hashCode = typeof(NoOpServiceProvider).GetHashCode() + 1;

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NoOpServiceProvider Instance { get; } = new();

        /// <summary>
        /// Initializes a new instance of <see cref="NoOpServiceProvider"/>.
        /// </summary>
        public NoOpServiceProvider()
        {

        }

        /// <inheritdoc/>
        public object? GetKeyedService(Type serviceType, object? serviceKey)
        {
            return null;
        }

        public object GetRequiredKeyedService(Type serviceType, object? serviceKey)
        {
            throw new InvalidOperationException($"No service registered for type '{serviceType}' and key '{serviceKey}'.");
        }

        public object? GetService(Type serviceType)
        {
            return null;
        }

        public override bool Equals(object? obj)
        {
            return obj is NoOpServiceProvider;
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }
    }
}

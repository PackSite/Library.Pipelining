namespace PackSite.Library.Pipelining.StepActivators
{
    using Microsoft.Extensions.DependencyInjection;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Internal;

    /// <summary>
    /// <see cref="ActivatorUtilities"/> based step activator.
    /// </summary>
    public sealed class ActivatorUtilitiesStepActivator : IStepActivator
    {
        private static readonly ConcurrentDictionary<Type, ObjectFactory> _cache = new();
        private static readonly IServiceProvider _serviceProvider = NoOpServiceProvider.Instance;

        /// <summary>
        /// Initializes a new instance of <see cref="ActivatorUtilitiesStepActivator"/>
        /// </summary>
        public ActivatorUtilitiesStepActivator()
        {

        }

        /// <inheritdoc/>
        public IBaseStep Create(Type stepType)
        {
            ObjectFactory stepFactory = _cache.GetOrAdd(stepType, (key) => // The factory may run multiple times but we don't care since we don't want to add overhead with lazy
            {
                return ActivatorUtilities.CreateFactory(key, Array.Empty<Type>());
            });

            return stepFactory(_serviceProvider, null) as IBaseStep ??
                throw new InvalidOperationException($"Failed to activate '{stepType.FullName ?? stepType.Name}'");
        }
    }
}

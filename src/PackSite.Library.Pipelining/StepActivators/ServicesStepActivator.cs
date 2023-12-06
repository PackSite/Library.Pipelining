namespace PackSite.Library.Pipelining.StepActivators
{
    using Microsoft.Extensions.DependencyInjection;
    using PackSite.Library.Pipelining;

    /// <summary>
    /// <see cref="IServiceProvider"/> and <see cref="ActivatorUtilities"/> based step activator.
    /// </summary>
    public sealed class ServicesStepActivator : IStepActivator
    {
        private readonly IServiceProvider _serviceProvider;

        private static readonly ConcurrentDictionary<Type, ObjectFactory> _cache = new();

        /// <summary>
        /// Initializes a new instance of <see cref="ServicesStepActivator"/>
        /// </summary>
        public ServicesStepActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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

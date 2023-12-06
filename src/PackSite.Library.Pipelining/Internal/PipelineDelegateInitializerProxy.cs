namespace PackSite.Library.Pipelining.Internal
{
    internal static class PipelineDelegateInitializerProxy
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Simple"/>.
        /// </summary>
        /// <param name="initializerDelegate"></param>
        public sealed class Simple(
            Action<IPipelineCollection>? initializerDelegate) : IPipelineInitializer
        {
            private readonly Action<IPipelineCollection>? _initializerDelegate = initializerDelegate;

            /// <inheritdoc/>
            public ValueTask RegisterAsync(IPipelineCollection pipelines, CancellationToken cancellationToken)
            {
                _initializerDelegate?.Invoke(pipelines);

                return default;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Complex"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="initializerDelegate"></param>
        public sealed class Complex(
            IServiceProvider serviceProvider,
            Func<IServiceProvider, IPipelineCollection, CancellationToken, ValueTask>? initializerDelegate) : IPipelineInitializer
        {
            private readonly IServiceProvider _serviceProvider = serviceProvider;
            private readonly Func<IServiceProvider, IPipelineCollection, CancellationToken, ValueTask>? _initializerDelegate = initializerDelegate;

            /// <inheritdoc/>
            public async ValueTask RegisterAsync(IPipelineCollection pipelines, CancellationToken cancellationToken)
            {
                if (_initializerDelegate is not null)
                {
                    await _initializerDelegate.Invoke(_serviceProvider, pipelines, cancellationToken);
                }
            }
        }
    }
}

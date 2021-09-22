namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class FullPipelineInitializerProxy : IPipelineInitializer
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<IServiceProvider, IPipelineCollection, CancellationToken, ValueTask>? _initializerDelegate;

        /// <summary>
        /// Initializes a new instance of <see cref="FullPipelineInitializerProxy"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="initializerDelegate"></param>
        public FullPipelineInitializerProxy(IServiceProvider serviceProvider, Func<IServiceProvider, IPipelineCollection, CancellationToken, ValueTask>? initializerDelegate)
        {
            _serviceProvider = serviceProvider;
            _initializerDelegate = initializerDelegate;
        }

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

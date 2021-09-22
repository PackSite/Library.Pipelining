namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class PipelineDelegateInitializerProxy
    {
        public sealed class Simple : IPipelineInitializer
        {
            private readonly Action<IPipelineCollection>? _initializerDelegate;

            /// <summary>
            /// Initializes a new instance of <see cref="Simple"/>.
            /// </summary>
            /// <param name="initializerDelegate"></param>
            public Simple(Action<IPipelineCollection>? initializerDelegate)
            {
                _initializerDelegate = initializerDelegate;
            }

            /// <inheritdoc/>
            public ValueTask RegisterAsync(IPipelineCollection pipelines, CancellationToken cancellationToken)
            {
                _initializerDelegate?.Invoke(pipelines);

                return default;
            }
        }

        public sealed class Complex : IPipelineInitializer
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly Func<IServiceProvider, IPipelineCollection, CancellationToken, ValueTask>? _initializerDelegate;

            /// <summary>
            /// Initializes a new instance of <see cref="Complex"/>.
            /// </summary>
            /// <param name="serviceProvider"></param>
            /// <param name="initializerDelegate"></param>
            public Complex(IServiceProvider serviceProvider, Func<IServiceProvider, IPipelineCollection, CancellationToken, ValueTask>? initializerDelegate)
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
}

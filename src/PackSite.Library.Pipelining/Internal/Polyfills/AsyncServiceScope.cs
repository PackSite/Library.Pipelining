#if !NET6_0_OR_GREATER
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// An <see cref="IServiceScope" /> implementation that implements <see cref="IAsyncDisposable" />.
    /// </summary>
    internal readonly struct AsyncServiceScope : IServiceScope, IAsyncDisposable
    {
        private readonly IServiceScope _serviceScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncServiceScope"/> struct.
        /// Wraps an instance of <see cref="IServiceScope" />.
        /// </summary>
        /// <param name="serviceScope">The <see cref="IServiceScope"/> instance to wrap.</param>
        public AsyncServiceScope(IServiceScope serviceScope)
        {
            _serviceScope = serviceScope ?? throw new ArgumentNullException(nameof(serviceScope));
        }

        /// <inheritdoc />
        public IServiceProvider ServiceProvider => _serviceScope.ServiceProvider;

        /// <inheritdoc />
        public void Dispose()
        {
            _serviceScope.Dispose();
        }

        /// <inheritdoc />
        public ValueTask DisposeAsync()
        {
            if (_serviceScope is IAsyncDisposable ad)
            {
                return ad.DisposeAsync();
            }
            _serviceScope.Dispose();

            // ValueTask.CompletedTask is only available in net5.0 and later.
            return default;
        }
    }
}
#endif
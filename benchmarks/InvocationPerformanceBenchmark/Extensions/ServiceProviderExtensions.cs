namespace InvocationPerformanceBenchmark.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    internal static class ServiceProviderExtensions
    {
        public static async Task FakeHostLifecycleAsync(this IServiceProvider serviceProvider, Func<CancellationToken, Task> func, CancellationToken cancellationToken = default)
        {
            IEnumerable<IHostedService> hostedServices = serviceProvider.GetServices<IHostedService>();

            foreach (IHostedService hostedService in hostedServices)
            {
                await hostedService.StartAsync(cancellationToken);
            }

            await func(cancellationToken);

            foreach (IHostedService hostedService in hostedServices)
            {
                await hostedService.StopAsync(cancellationToken);
            }
        }

        public static async Task FakeHostStartAsync(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            IEnumerable<IHostedService> hostedServices = serviceProvider.GetServices<IHostedService>();

            foreach (IHostedService hostedService in hostedServices)
            {
                await hostedService.StartAsync(cancellationToken);
            }
        }

        public static async Task FakeHostStopAsync(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            IEnumerable<IHostedService> hostedServices = serviceProvider.GetServices<IHostedService>();

            foreach (IHostedService hostedService in hostedServices)
            {
                await hostedService.StopAsync(cancellationToken);
            }
        }
    }
}

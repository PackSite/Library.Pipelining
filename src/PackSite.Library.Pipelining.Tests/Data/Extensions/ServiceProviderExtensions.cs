namespace PackSite.Library.Pipelining.Tests.Data.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    internal static class ServiceProviderExtensions
    {
        public static async Task FakeHostStartupAsync(this IServiceProvider serviceProvider, Func<CancellationToken, Task> func, CancellationToken cancellationToken = default)
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
    }
}

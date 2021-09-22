namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    internal sealed class PipeliningHostedService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IPipelineCollection _pipelineCollection;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="PipeliningHostedService"/>.
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        /// <param name="pipelineCollection"></param>
        /// <param name="logger"></param>
        public PipeliningHostedService(IServiceScopeFactory serviceScopeFactory, IPipelineCollection pipelineCollection, ILoggerFactory logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _pipelineCollection = pipelineCollection;
            _logger = logger.CreateLogger("PackSite.Library.Pipelining");
        }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Initializing PackSite.Library.Pipelining");
            Stopwatch stopwatch = Stopwatch.StartNew();

            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                IEnumerable<IPipelineInitializer> initializers = scope.ServiceProvider.GetServices<IPipelineInitializer>();

                foreach (IPipelineInitializer pi in initializers)
                {
                    await pi.RegisterAsync(_pipelineCollection, cancellationToken);
                }
            }
            stopwatch.Stop();

            _logger.LogInformation("Succesfully initialized PackSite.Library.Pipelining after {Elapsed}", stopwatch.Elapsed);
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _pipelineCollection.Clear();
            Log.ClearedPipelines(_logger);

            return Task.CompletedTask;
        }

        /// <summary>
        /// High-perfomrance logging definitions.
        /// </summary>
        private static class Log
        {
            private const int PackSiteEventId = 8083;

            private static readonly Action<ILogger, Exception> _clearedPipelines =
                LoggerMessage.Define(
                    LogLevel.Debug,
                    new EventId(PackSiteEventId, "ClearedPipelines"),
                    "Cleared pipelines");

            public static void ClearedPipelines(ILogger logger)
            {
                _clearedPipelines(logger, null!);
            }
        }
    }
}

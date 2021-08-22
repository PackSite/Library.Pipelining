namespace PackSite.Library.Pipelining.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining.Configuration;
    using PackSite.Library.Pipelining.Internal.Extensions;

    internal sealed class PipeliningHostedService : IHostedService, IDisposable
    {
        private IReadOnlyList<PipelineName> _lastRegistered = new List<PipelineName>();

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private PipeliningConfiguration _options;
        private readonly IDisposable _optionsMonitor;
        private readonly IPipelineCollection _pipelineCollection;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="PipeliningHostedService"/>.
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        /// <param name="options"></param>
        /// <param name="pipelineCollection"></param>
        /// <param name="logger"></param>
        public PipeliningHostedService(IServiceScopeFactory serviceScopeFactory, IOptionsMonitor<PipeliningConfiguration> options, IPipelineCollection pipelineCollection, ILogger<PipeliningHostedService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _options = options.CurrentValue;
            _optionsMonitor = options.OnChange(OptionsChanged);

            _pipelineCollection = pipelineCollection;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Initializing PackSite.Library.Pipelining");

            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                IEnumerable<IPipelineInitializer> initializers = scope.ServiceProvider.GetServices<IPipelineInitializer>();

                foreach (IPipelineInitializer pi in initializers)
                {
                    await pi.RegisterAsync(_pipelineCollection, cancellationToken);
                }
            }

            UpdatePipelines();

            _logger.LogInformation("Succesfully initialized PackSite.Library.Pipelining");
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (PipelineName toRemove in _lastRegistered)
            {
                _pipelineCollection.TryRemove(toRemove);
                Log.RemovedPipeline(_logger, toRemove);
            }

            _lastRegistered = new List<PipelineName>();

            return Task.CompletedTask;
        }

        private void OptionsChanged(PipeliningConfiguration @new, string namedOptions)
        {
            lock (_optionsMonitor)
            {
                _options = @new;
                UpdatePipelines();
            }
        }

        private void UpdatePipelines()
        {
            try
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();

                IReadOnlyList<IPipeline> pipelines = _options.BuildPipelines();
                List<PipelineName> toPreserve = new(pipelines.Count);

                foreach (IPipeline pipeline in pipelines)
                {
                    _pipelineCollection.AddOrReplace(pipeline);
                    toPreserve.Add(pipeline.Name);

                    Log.UpdatedPipeline(_logger, pipeline.Name);
                }

                IReadOnlyList<PipelineName> pipelinesToRemove = _lastRegistered.Except(toPreserve).ToList();
                foreach (PipelineName toRemove in pipelinesToRemove)
                {
                    _pipelineCollection.TryRemove(toRemove);
                    Log.RemovedPipeline(_logger, toRemove);
                }

                _lastRegistered = toPreserve;

                stopwatch.Stop();
                Log.Updated(_logger, stopwatch.Elapsed, pipelines.Count, pipelinesToRemove.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update pipelines from configuration");

                if (_options.ThrowOnReloadError)
                {
                    throw;
                }
            }
        }

        public void Dispose()
        {
            _optionsMonitor.Dispose();
        }

        /// <summary>
        /// High-perfomrance logging definitions.
        /// </summary>
        private static class Log
        {
            private const int PackSiteEventId = 8083;

            private static readonly Action<ILogger, string, Exception> _removedPipeline =
                LoggerMessage.Define<string>(
                    LogLevel.Debug,
                    new EventId(PackSiteEventId, "RemovedPipeline"),
                    "Removed pipeline '{Name}'");

            private static readonly Action<ILogger, string, Exception> _updatedPipeline =
                LoggerMessage.Define<string>(
                    LogLevel.Debug,
                    new EventId(PackSiteEventId, "UpdatedPipeline"),
                    "Updated pipeline '{Name}'");

            private static readonly Action<ILogger, TimeSpan, int, int, Exception> _updated =
                LoggerMessage.Define<TimeSpan, int, int>(
                    LogLevel.Information,
                    new EventId(PackSiteEventId, "Updated"),
                    "Updated pipelines collection after {Elapsed} (Inserted: {InsertedCount}; Removed: {RemovedCount})");

            public static void RemovedPipeline(ILogger logger, PipelineName pipelineName)
            {
                _removedPipeline(logger, pipelineName, null!);
            }

            public static void UpdatedPipeline(ILogger logger, PipelineName pipelineName)
            {
                _updatedPipeline(logger, pipelineName, null!);
            }

            public static void Updated(ILogger logger, TimeSpan elapsed, int inserted, int removed)
            {
                _updated(logger, elapsed, inserted, removed, null!);
            }
        }
    }
}

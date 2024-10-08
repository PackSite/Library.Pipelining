﻿namespace PackSite.Library.Pipelining.Configuration.Internal
{
    using System.Diagnostics;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Configuration;
    using PackSite.Library.Pipelining.Configuration.Extensions;

    /// <summary>
    /// Initializes a new instance of <see cref="PipeliningConfigurationHostedService"/>.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="pipelineCollection"></param>
    /// <param name="loggerFactory"></param>
    internal sealed class PipeliningConfigurationHostedService(
        IOptionsMonitor<PipeliningConfiguration> options,
        IPipelineCollection pipelineCollection,
        ILoggerFactory loggerFactory) : IHostedService, IDisposable
    {
        private IReadOnlyList<PipelineName> _lastRegistered = [];
        private IDisposable? _optionsMonitor;

        private readonly SemaphoreSlim _lock = new(1, 1);

        private readonly IOptionsMonitor<PipeliningConfiguration> _options = options;
        private readonly IPipelineCollection _pipelineCollection = pipelineCollection;
        private readonly ILogger _logger = loggerFactory.CreateLogger("PackSite.Library.Pipelining.Configuration");

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Initializing PackSite.Library.Pipelining.Configuration");

            await UpdatePipelinesAsync(_options.CurrentValue, cancellationToken);
            _optionsMonitor = _options.OnChange(OptionsChanged);

            stopwatch.Stop();
            _logger.LogInformation("Successfully initialized PackSite.Library.Pipelining.Configuration after {Elapsed}", stopwatch.Elapsed);
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _lastRegistered = [];

            return Task.CompletedTask;
        }

        private void OptionsChanged(PipeliningConfiguration pipeliningConfiguration, string? namedOptions)
        {
            UpdatePipelinesAsync(pipeliningConfiguration, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private async Task UpdatePipelinesAsync(PipeliningConfiguration pipeliningConfiguration, CancellationToken cancellationToken)
        {
            try
            {
                await _lock.WaitAsync(cancellationToken);
                Stopwatch stopwatch = Stopwatch.StartNew();

                IReadOnlyList<IPipeline> pipelines = pipeliningConfiguration.BuildPipelines();
                List<PipelineName> toPreserve = new(pipelines.Count);

                int added = 0;
                int updated = 0;

                foreach (IPipeline pipeline in pipelines)
                {
                    bool wasAdded = _pipelineCollection.AddOrUpdate(pipeline);
                    toPreserve.Add(pipeline.Name);

                    if (wasAdded)
                    {
                        ++added;
                    }
                    else
                    {
                        ++updated;
                    }

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
                Log.Updated(_logger, stopwatch.Elapsed, added, updated, pipelinesToRemove.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update pipelines from configuration");

                if (pipeliningConfiguration.ThrowOnReloadError)
                {
                    throw;
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public void Dispose()
        {
            _lock.Dispose();
            _optionsMonitor?.Dispose();
        }

        /// <summary>
        /// High-performance logging definitions.
        /// </summary>
        private static class Log
        {
            private const int PackSiteEventId = 8083;

            private static readonly Action<ILogger, string, Exception?> _removedPipeline =
                LoggerMessage.Define<string>(
                    LogLevel.Debug,
                    new EventId(PackSiteEventId, nameof(RemovedPipeline)),
                    "Removed pipeline '{Name}'");

            private static readonly Action<ILogger, Exception?> _clearedPipelines =
                LoggerMessage.Define(
                    LogLevel.Debug,
                    new EventId(PackSiteEventId, nameof(ClearedPipelines)),
                    "Cleared pipelines");

            private static readonly Action<ILogger, string, Exception?> _updatedPipeline =
                LoggerMessage.Define<string>(
                    LogLevel.Debug,
                    new EventId(PackSiteEventId, nameof(UpdatedPipeline)),
                    "Updated pipeline '{Name}'");

            private static readonly Action<ILogger, TimeSpan, int, int, int, Exception?> _updated =
                LoggerMessage.Define<TimeSpan, int, int, int>(
                    LogLevel.Information,
                    new EventId(PackSiteEventId, nameof(Updated)),
                    "Updated pipelines collection after {Elapsed} (Added: {AddedCount}; Updated: {UpdatedCount}; Removed: {RemovedCount})");

            public static void RemovedPipeline(ILogger logger, PipelineName pipelineName)
            {
                _removedPipeline(logger, pipelineName, null);
            }

            public static void ClearedPipelines(ILogger logger)
            {
                _clearedPipelines(logger, null);
            }

            public static void UpdatedPipeline(ILogger logger, PipelineName pipelineName)
            {
                _updatedPipeline(logger, pipelineName, null);
            }

            public static void Updated(ILogger logger, TimeSpan elapsed, int addded, int updated, int removed)
            {
                _updated(logger, elapsed, addded, updated, removed, null);
            }
        }
    }
}

namespace PackSite.Library.Pipelining.Configuration.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Configuration;
    using PackSite.Library.Pipelining.Configuration.Extensions;

    internal sealed class PipeliningConfigurationHostedService : IHostedService, IDisposable
    {
        private IReadOnlyList<PipelineName> _lastRegistered = new List<PipelineName>();

        private readonly IOptionsMonitor<PipeliningConfiguration> _options;
        private readonly IDisposable _optionsMonitor;
        private readonly IPipelineCollection _pipelineCollection;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="PipeliningConfigurationHostedService"/>.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="pipelineCollection"></param>
        /// <param name="loggerFactory"></param>
        public PipeliningConfigurationHostedService(IOptionsMonitor<PipeliningConfiguration> options, IPipelineCollection pipelineCollection, ILoggerFactory loggerFactory)
        {
            _options = options;
            _optionsMonitor = options.OnChange(OptionsChanged);

            _pipelineCollection = pipelineCollection;
            _logger = loggerFactory.CreateLogger("PackSite.Library.Pipelining.Configuration");
        }

        /// <inheritdoc/>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            _logger.LogInformation("Initializing PackSite.Library.Pipelining.Configuration");

            lock (_optionsMonitor)
            {
                UpdatePipelines(_options.CurrentValue);
            }

            stopwatch.Stop();
            _logger.LogInformation("Succesfully initialized PackSite.Library.Pipelining.Configuration after {Elapsed}", stopwatch.Elapsed);

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _lastRegistered = new List<PipelineName>();

            return Task.CompletedTask;
        }

        private void OptionsChanged(PipeliningConfiguration pipeliningConfiguration, string namedOptions)
        {
            lock (_optionsMonitor)
            {
                UpdatePipelines(pipeliningConfiguration);
            }
        }

        private void UpdatePipelines(PipeliningConfiguration pipeliningConfiguration)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                IReadOnlyList<IPipeline> pipelines = pipeliningConfiguration.BuildPipelines();
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

                if (pipeliningConfiguration.ThrowOnReloadError)
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

            private static readonly Action<ILogger, Exception> _clearedPipelines =
                LoggerMessage.Define(
                    LogLevel.Debug,
                    new EventId(PackSiteEventId, "ClearedPipelines"),
                    "Cleared pipelines");

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

            public static void ClearedPipelines(ILogger logger)
            {
                _clearedPipelines(logger, null!);
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

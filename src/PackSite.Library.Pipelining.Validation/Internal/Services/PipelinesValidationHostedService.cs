namespace PackSite.Library.Pipelining.Validation.Internal.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using PackSite.Library.Pipelining;
    using PackSite.Library.Pipelining.Validation;
    using PackSite.Library.Pipelining.Validation.Internal.Utils;
    using PackSite.Library.Pipelining.Validation.Validators;

    internal sealed class PipelinesValidationHostedService : IHostedService, IDisposable
    {
        private readonly IPipelineCollection _pipelineCollection;
        private readonly IPipelinesValidationService _pipelinesValidationService;
        private readonly IEnumerable<IValidator> _validators;

        private PipelinesValidationOptions _options;
        private readonly IDisposable _optionsMonitor;

        private readonly ILogger _startupLogger;
        private readonly ILogger _reloadLogger;

        /// <summary>
        /// Initializes a new instance of <see cref="PipelinesValidationHostedService"/>.
        /// </summary>
        public PipelinesValidationHostedService(IPipelineCollection pipelineCollection,
                                                IPipelinesValidationService pipelinesValidationService,
                                                IOptionsMonitor<PipelinesValidationOptions> options,
                                                IEnumerable<IValidator> validators,
                                                ILoggerFactory loggerFactory)
        {
            _pipelineCollection = pipelineCollection;
            _pipelinesValidationService = pipelinesValidationService;
            _validators = validators;

            _options = options.CurrentValue;
            _optionsMonitor = options.OnChange((@new, namedOptions) => _options = @new);

            _startupLogger = loggerFactory.CreateLogger("PackSite.Library.Pipelining.Validation.Startup");
            _reloadLogger = loggerFactory.CreateLogger("PackSite.Library.Pipelining.Validation.Reload");

            pipelineCollection.Changed += PipelineCollection_Changed;
        }

        private void PipelineCollection_Changed(object? sender, EventArgs e)
        {
            if (_options.ValidateOnCollectionChange)
            {
                AsyncUtil.RunSync(async () =>
                {
                    Log.ValidatingPipelinesOnCollectionChange(_reloadLogger);
                    await TryValidateAsync(_reloadLogger, CancellationToken.None);
                });
            }
        }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_options.ValidateOnStartup)
            {
                Log.ValidatingPipelinesOnStartup(_startupLogger);
                await TryValidateAsync(_startupLogger, cancellationToken);
            }
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _pipelineCollection.Changed -= PipelineCollection_Changed;
            _optionsMonitor.Dispose();
        }

        private async Task TryValidateAsync(ILogger logger, CancellationToken cancellationToken)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                await _pipelinesValidationService.ValidateAndThrowAsync(_pipelineCollection, _validators, cancellationToken);
            }
            catch (ValidationException pex)
            {
                Log.ValidationFailure(logger, pex);
            }
            catch (Exception ex)
            {
                Log.UnknownError(logger, ex);
            }
            finally
            {
                stopwatch.Stop();
                Log.ValidatedPipelines(logger, stopwatch.Elapsed);
            }
        }

        /// <summary>
        /// High-performance logging definitions.
        /// </summary>
        private static class Log
        {
            private const int PackSiteEventId = 8083;

            private static readonly Action<ILogger, Exception?> _validatingPipelinesOnStartup =
                LoggerMessage.Define(
                    LogLevel.Debug,
                    new EventId(PackSiteEventId, nameof(ValidatingPipelinesOnStartup)),
                    "Validating pipelines on app startup");

            private static readonly Action<ILogger, Exception?> _validatingPipelinesOnCollectionChange =
                LoggerMessage.Define(
                    LogLevel.Debug,
                    new EventId(PackSiteEventId, nameof(ValidatingPipelinesOnCollectionChange)),
                    "Validating pipelines after collection change");

            private static readonly Action<ILogger, ValidationResult, Exception?> _validationFailure =
                LoggerMessage.Define<ValidationResult>(
                    LogLevel.Error,
                    new EventId(PackSiteEventId, nameof(ValidatingPipelinesOnStartup)),
                    "Validation failed for one or more pipelines: {@Result}");

            private static readonly Action<ILogger, Exception?> _unknownError =
                LoggerMessage.Define(
                    LogLevel.Critical,
                    new EventId(PackSiteEventId, nameof(UnknownError)),
                    "Unknown error occurred during pipelines validation");

            private static readonly Action<ILogger, TimeSpan, Exception?> _validatedPipelines =
                LoggerMessage.Define<TimeSpan>(
                    LogLevel.Information,
                    new EventId(PackSiteEventId, nameof(ValidatedPipelines)),
                    "Validated pipelines after {Elapsed}");

            public static void ValidatingPipelinesOnStartup(ILogger logger)
            {
                _validatingPipelinesOnStartup(logger, null);
            }

            public static void ValidatingPipelinesOnCollectionChange(ILogger logger)
            {
                _validatingPipelinesOnCollectionChange(logger, null);
            }

            public static void ValidationFailure(ILogger logger, ValidationException ex)
            {
                _validationFailure(logger, ex.ValidationResult, ex);
            }

            public static void UnknownError(ILogger logger, Exception? ex)
            {
                _unknownError(logger, ex);
            }

            public static void ValidatedPipelines(ILogger logger, TimeSpan elapsed)
            {
                _validatedPipelines(logger, elapsed, null);
            }
        }
    }
}

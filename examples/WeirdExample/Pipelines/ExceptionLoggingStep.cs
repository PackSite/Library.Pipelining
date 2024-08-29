namespace WeirdExample.Pipelines
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Pipelining;

    public class ExceptionLoggingStep : IStep
    {
        private readonly ILogger _logger;

        public ExceptionLoggingStep(ILogger<ExceptionLoggingStep> logger)
        {
            _logger = logger;
        }

        public async Task ExecuteAsync(object args, StepDelegate next, IInvokablePipeline invokablePipeline, CancellationToken cancellationToken = default)
        {
            try
            {
                await next();
            }
            catch (InvalidOperationException)
            {
                _logger.LogError("{ExceptionType} handled by {Step}", nameof(InvalidOperationException), nameof(ExceptionLoggingStep));

                // Retry
                //await invokablePipeline.InvokeAsync(args, cancellationToken);
            }
        }
    }
}

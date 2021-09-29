namespace PipelineInitializerExample.Steps
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Pipelining;

    public class ExceptionHandlingStep : IStep
    {
        private readonly ILogger _logger;

        public ExceptionHandlingStep(ILogger<ExceptionHandlingStep> logger)
        {
            _logger = logger;
        }

        public async ValueTask ExecuteAsync(object args, StepDelegate next, IInvokablePipeline invokablePipeline, CancellationToken cancellationToken = default)
        {
            try
            {
                await next();
            }
            catch (Exception)
            {
                _logger.LogError("An exception occured while executing '{0}' pipeline", invokablePipeline.Pipeline.Name);

                // Retry
                //await invokablePipeline.InvokeAsync(args, cancellationToken);
            }
        }
    }
}

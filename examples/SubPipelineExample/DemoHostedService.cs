namespace SubPipelineExample
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using PackSite.Library.Pipelining;
    using SubPipelineExample.Pipelines;

    public sealed class DemoHostedService : BackgroundService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DemoHostedService(IHostApplicationLifetime appLifetime, ILogger<DemoHostedService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _appLifetime = appLifetime;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                try
                {
                    await using (AsyncServiceScope scope = _serviceScopeFactory.CreateAsyncScope())
                    {
                        DemoArgs pipelineArgs = new();

                        IInvokablePipelineFactory invokablePipelineFactory = scope.ServiceProvider.GetRequiredService<IInvokablePipelineFactory>();
                        IInvokablePipeline<DemoArgs> invokablePipeline = invokablePipelineFactory.GetRequiredPipeline<DemoArgs>();

                        await invokablePipeline.InvokeAsync(pipelineArgs, stoppingToken);
                        _logger.LogInformation("OUTPUT: {Output}", pipelineArgs.Value);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Fatal demo error");
                }
                finally
                {
                    _appLifetime.StopApplication();
                }
            }, stoppingToken);
        }
    }
}
